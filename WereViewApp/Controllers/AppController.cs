#region using block

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.Derivables;
using WereViewApp.Models.EntityModel.ExtenededWithCustomMethods;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Models.ViewModels;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Uploads;
using WereViewApp.WereViewAppCommon;
using WereViewApp.WereViewAppCommon.Structs;

#endregion

namespace WereViewApp.Controllers {
    [Authorize]
    public class AppController : AdvanceController {
        #region Declaration

        private readonly Algorithms _algorithms = new Algorithms();

        #endregion

        #region Constructors

        public AppController()
            : base(true) {
            ViewBag.controller = ControllerName;
        }

        #endregion

        #region Single App Display Page

        [AllowAnonymous]
        [OutputCache(CacheProfile = "Short", VaryByParam = "platform;platformVersion;category;url")]
        public ActionResult SingleAppDisplay(string platform, float platformVersion, string category, string url) {
            var app = _algorithms.GetSingleAppForDisplay(platform, platformVersion, category, url, 30, db);
            if (app != null) {
                _algorithms.IncreaseViewCount(app, db);
                return View(app);
            }
            return View("_AppNotFound");
        }

        #endregion

        #region App Not Found

        public ActionResult AppNotFound() {
            return View("_AppNotFound");
        }

        #endregion

        #region Delete Gallery Image

        //[ValidateAntiForgeryToken]
        [HttpGet]
        public JsonResult DeleteGalleryImage(Guid uploadGuid, byte sequence, string requestVerificationToken) {
            var gallery = db.Galleries.FirstOrDefault(n => n.UploadGuid == uploadGuid && n.Sequence == sequence);
            if (gallery != null) {
                var fileName = WereViewStatics.uProcessorGallery.GetOrganizeName(gallery, true);
                var absPath =
                    WereViewStatics.uProcessorGallery.VirtualPathtoAbsoluteServerPath(
                        WereViewStatics.uProcessorGallery.GetCombinationOfRootAndAdditionalRoot() + fileName);
                if (System.IO.File.Exists(absPath)) {
                    System.IO.File.Delete(absPath);
                }

                absPath =
                    WereViewStatics.uProcessorGallery.VirtualPathtoAbsoluteServerPath(
                        WereViewStatics.uProcessorGallery.GetCombinationOfRootAndAdditionalRoot() + fileName);
                if (System.IO.File.Exists(absPath)) {
                    System.IO.File.Delete(absPath);
                }

                ResetSessionForUploadSequence(uploadGuid);

                //find  the temp
                var temp = db.TempUploads.FirstOrDefault(n => n.GalleryID == gallery.GalleryID);
                if (temp != null) {
                    db.TempUploads.Remove(temp);
                }
                db.Galleries.Remove(gallery);
                db.SaveChanges();
                Session[uploadGuid.ToString()] = null;

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Upload Edit Btn

        [ValidateAntiForgeryToken]
        public ActionResult EditGalleryUploads(App app) {
            var token = Request["__RequestVerificationToken"];
            var path = AppVar.Url + WereViewStatics.uProcessorGallery.RootPath.Replace("~/", "/") +
                       WereViewStatics.uProcessorGallery.AdditionalRoots;

            var uploadedImages = db.Galleries
                .Where(n => n.GalleryCategoryID == GalleryCategoryIDs.AppPageGallery && n.UploadGuid == app.UploadGuid)
                .OrderBy(n => n.Sequence)
                .AsEnumerable()
                .Select(n => new UploadedGalleryImageEditViewModel {
                    Tile = n.Title,
                    Subtitle = n.Subtitle,
                    Sequence = n.Sequence,
                    UploadGuid = n.UploadGuid,
                    Id = app.AppID,
                    ImageURL = path + UploadProcessor.GetOrganizeNameStatic(n, true, false, ""),
                    DeleteURL = "/" + ControllerName +
                                "/DeleteGalleryImage?uploadGuid=" + n.UploadGuid +
                                "&sequence=" + n.Sequence +
                                "&__RequestVerificationToken=" + token
                    //ReUploadURL = "/" + _controllerName +
                    //"/ReuploadGalleryImage?uploadGuid=" + n.UploadGuid +
                    //"&sequence=" + n.Sequence +
                    //"&__RequestVerificationToken=" + token
                }).ToList();


            return View(uploadedImages);
        }

        #endregion

        #region Count of Gallery Images

        private int GetAppGalleryImageCountValueStatic(Guid uploadGuid) {
            if (uploadGuid == null) {
                return 0;
            }
            var sessionName = uploadGuid + "-staticCount";

            var sessionCount = Session[sessionName];
            if (sessionCount != null) {
                var b = byte.Parse(sessionCount.ToString());
                return b;
            }
            //try to get from the database.
            var count =
                db.Galleries.Count(
                    n => n.UploadGuid == uploadGuid && n.GalleryCategoryID == GalleryCategoryIDs.AppPageGallery);
            Session[sessionName] = count;
            return count;
        }

        #endregion

        #region View tapping

        /// <summary>
        ///     Always tap once before going into the view.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="App">Gives the model if it is a editing state or creating posting state or when deleting.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        private bool ViewTapping(ViewStates view, App app = null) {
            switch (view) {
                case ViewStates.Index:
                    break;
                case ViewStates.Create:
                    ViewBag.ShowAllDivs = "false";
                    break;
                case ViewStates.CreatePost: // before saving it
                    ViewBag.ShowAllDivs = "true";
                    break;
                case ViewStates.Edit:
                    ViewBag.ShowAllDivs = "true";
                    ViewBag.preexistGallery = GetAppGalleryImageCountValueStatic(app.UploadGuid);
                    break;
                case ViewStates.Details:
                    break;
                case ViewStates.EditPost: // before saving it
                    ViewBag.preexistGallery = GetAppGalleryImageCountValueStatic(app.UploadGuid);
                    ViewBag.ShowAllDivs = "true";
                    break;
                case ViewStates.SavingAppDraftBeforePost: // before saving it

                    break;
                case ViewStates.SavingAppDraftAfterPost: // before saving it
                    break;

                case ViewStates.DraftsIndex:
                    break;

                case ViewStates.DraftsToApp:
                    ViewBag.preexistGallery = GetAppGalleryImageCountValueStatic(app.UploadGuid);
                    if (ViewBag.preexistGallery > 0) {
                        ViewBag.ShowAllDivs = "true";
                    }
                    break;
                case ViewStates.Delete:
                    break;
            }
            return true;
        }

        #endregion

        #region Save database common method

        /// <summary>
        ///     Better approach to save things into database(than db.SaveChanges()) for this controller.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="App">Your model information to send in email to developer when failed to save.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        private bool SaveDatabase(ViewStates view, IApp app = null) {
            // working those at HttpPost time.
            switch (view) {
                case ViewStates.Create:
                    break;
                case ViewStates.Edit:
                    break;
                case ViewStates.Delete:
                    break;
            }

            try {
                var changes = db.SaveChanges(app);
                if (changes > 0) {
                    return true;
                }
            } catch (Exception ex) {
                throw new Exception("Message : " + ex.Message + " Inner Message : " + ex.InnerException.Message);
            }
            return false;
        }

        #endregion

        #region Saving App as Draft

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveDraft(App app) {
            var userId = UserManager.GetLoggedUserId();
            if (app.AppID != 0) {
                // app id exist that means 
                // app is already saved in the database.
                return Json(false, "text/html");
                ;
            }
            if (app.UploadGuid != null) {
                //if (db.AppDrafts.Any(n => n.PostedByUserID == userId)) {
                ViewTapping(ViewStates.SavingAppDraftBeforePost, app);
                var count = db.AppDrafts.Count(n => n.PostedByUserID == userId);

                if (count > AppVar.Setting.MaxDraftPostByUsers) {
                    // buffer limit over.
                    return null;
                }
                //}

                var appDraft = await db.AppDrafts.FirstOrDefaultAsync(n => n.UploadGuid == app.UploadGuid);
                AddingNeccessaryWhilePosting(app);
                if (appDraft == null) {
                    // create one
                    appDraft = GetDraft(app);
                    db.AppDrafts.Add(appDraft);
                } else {
                    // modify existing one
                    GetDraft(app, appDraft);
                    //db.Entry(appDraft).State = EntityState.Modified;
                }

                ViewTapping(ViewStates.SavingAppDraftBeforePost, app);
                await db.SaveChangesAsync();
                return Json(true, "text/html");
            }
            return Json(false, "text/html");
        }

        #endregion

        #region Draft Index

        public ActionResult Drafts() {
            var user = UserManager.GetCurrentUser();
            long userId = -1;
            if (user != null) {
                userId = user.UserID;
            }
            var apps = db.AppDrafts.Where(n => n.PostedByUserID == userId);
            ViewTapping(ViewStates.DraftsIndex);

            return View(apps.ToList());
        }

        #endregion

        #region Draft->App

        public ActionResult Draft(Guid id) {
            var userId = UserManager.GetLoggedUserId();
            var draft = db.AppDrafts.FirstOrDefault(n => n.UploadGuid == id & n.PostedByUserID == userId);
            if (draft != null) {
                var app = GetAppfromDraft(draft);

                ViewTapping(ViewStates.DraftsToApp, app);

                GetDropDowns(app);
                return View("Post", app);
            }
            return HttpNotFound();
        }

        #endregion

        #region Draft Delete

        public ActionResult DeleteDraft(Guid id) {
            RemoveTempUploadAndDraftFromRelatedApp(id);
            return RedirectToAction("Drafts");
        }

        #endregion

        #region Manage : Tags

        /// <summary>
        ///     Called specifically from Post save or edit save
        ///     not from AdditionNeccessaryFields
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="uploadGuid"></param>
        /// <param name="tagString"></param>
        private void ManageTagsInDatabase(long appId, Guid uploadGuid, string tagString) {
            new Thread(() => {
                if (!string.IsNullOrWhiteSpace(tagString)) {
                    using (var db2 = new WereViewAppEntities()) {
                        var tags = tagString.Split(";,".ToCharArray());
                        foreach (var tag in tags) {
                            // remove any previous tag relation with this app.
                            // removing
                            db2.Database.ExecuteSqlCommand("DELETE FROM  TagAppRelation WHERE AppID=@p0", appId);

                            var tagFromDatabase = db2.Tags.FirstOrDefault(n => n.TagDisplay == tag);
                            if (tagFromDatabase == null) {
                                // creating tag
                                // if tag not exist in the database then create one.
                                tagFromDatabase = new Tag {
                                    TagDisplay = tag
                                };
                                db2.Tags.Add(tagFromDatabase);
                            }

                            db2.SaveChanges();

                            // add tag relation with this app
                            var newTagRel = new TagAppRelation();
                            newTagRel.TagID = tagFromDatabase.TagID;
                            newTagRel.AppID = appId;
                            tagFromDatabase.TagAppRelations.Add(newTagRel);
                            db2.SaveChanges();
                        }
                    }
                }
            }).Start();
        }

        #endregion

        #region Index

        public ActionResult Index() {
            var userId = UserManager.GetLoggedUserId();

            var apps = db.Apps.Include(a => a.Category).Include(a => a.Platform).Where(n => n.PostedByUserID == userId);
            var viewOf = ViewTapping(ViewStates.Index);
            return View(apps.ToList());
        }

        #endregion

        #region Details

        public ActionResult Details(Int64 id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var app = db.Apps.Find(id);
            if (app == null) {
                return HttpNotFound();
            }
            var viewOf = ViewTapping(ViewStates.Details, app);
            return View(app);
        }

        #endregion

        #region Removing output cache

        public void RemoveOutputCache(string url) {
            HttpResponse.RemoveOutputCacheItem(url);
        }

        #endregion

        #region Enums

        internal enum ViewStates {
            DisplayApp,
            Index,
            Create,
            CreatePost,
            Edit,
            EditPost,
            Details,
            Delete,
            DeletePost,
            SavingAppDraftBeforePost,
            SavingAppDraftAfterPost,
            DraftsIndex,
            DraftsToApp
        }

        #endregion

        #region Developer Comments - Alim Ul karim

        // Generated by Alim Ul Karim on behalf of Developers Organism.
        // Find us developers-organism.com
        // https://www.facebook.com/DevelopersOrganism
        // mailto:alim@developers-organism.com		

        #endregion

        #region Constants

        private const string DeletedError =
            "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";

        private const string DeletedSaved = "Removed successfully.";
        private const string EditedSaved = "Modified successfully.";

        private const string EditedError =
            "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";

        private const string CreatedError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        private const string CreatedSaved = "Transaction is successfully added to the database.";
        private const string ControllerName = "App";

        /// Constant value for where the controller is actually visible.
        private const string ControllerVisibleUrl = "";

        #endregion

        #region App Saved Messages

        private string PostedSuccessFully(string title, bool published) {
            if (!published) {
                return "Your new app '" + title +
                       "' has been saved successfully but not published. You can also save a new one right away.";
            }
            return "Your new app '" + title +
                   "' has been successfully saved and published. You can also save a new one right away.";
        }

        private string EditSuccessFully(string title, bool published) {
            if (!published) {
                return "Your app '" + title + "' has been modified successfully but not published.";
            }
            return "Your app '" + title + "' has been successfully modified and published.";
        }

        private string EditFailed(string title) {
            return "Your app '" + title + "' has been failed to modify.";
        }

        private string PostedFailed(string title) {
            return "Your new app '" + title + "' has been failed to save.";
        }

        #endregion

        #region App Draft Related Methods

        /// <summary>
        ///     returns draft if it is related to current user.
        /// </summary>
        /// <param name="draftId"></param>
        /// <returns></returns>
        private App GetAppfromDraft(AppDraft appDraft) {
            if (appDraft == null) {
                return null;
            }
            var rApp = new App();

            rApp.AppName = appDraft.AppName;
            rApp.CategoryID = appDraft.CategoryID;
            rApp.PlatformID = appDraft.PlatformID;
            rApp.PlatformVersion = (double) appDraft.PlatformVersion;
            rApp.Description = appDraft.Description;
            rApp.PostedByUserID = appDraft.PostedByUserID;
            rApp.IsVideoExist = (bool) appDraft.IsVideoExist;
            rApp.YoutubeEmbedLink = appDraft.YoutubeEmbedLink;
            rApp.WebSiteURL = appDraft.WebSiteURL;
            rApp.StoreURL = appDraft.StoreURL;
            rApp.IsBlocked = (bool) appDraft.IsBlocked;
            rApp.IsPublished = (bool) appDraft.IsPublished;
            rApp.UploadGuid = appDraft.UploadGuid;
            rApp.URL = appDraft.URL;
            rApp.ReleaseDate = appDraft.ReleaseDate;

            return rApp;
        }

        private AppDraft GetDraft(App app, AppDraft appDraft = null) {
            if (appDraft == null) {
                appDraft = new AppDraft();
            }
            appDraft.AppName = app.AppName;
            appDraft.CategoryID = app.CategoryID;
            appDraft.PlatformID = app.PlatformID;
            appDraft.PlatformVersion = app.PlatformVersion;
            appDraft.Description = app.Description;
            appDraft.PostedByUserID = app.PostedByUserID;
            appDraft.IsVideoExist = app.IsVideoExist;
            appDraft.YoutubeEmbedLink = app.YoutubeEmbedLink;
            appDraft.WebSiteURL = app.WebSiteURL;
            appDraft.StoreURL = app.StoreURL;
            appDraft.IsBlocked = app.IsBlocked;
            appDraft.IsPublished = app.IsPublished;
            appDraft.UploadGuid = app.UploadGuid;
            appDraft.URL = app.URL;
            appDraft.ReleaseDate = app.ReleaseDate;
            return appDraft;
        }

        #endregion

        #region Manage Virtual Fields : In File (Idea, Tags, Developers..)

        #region Saving asycnchronously

        /// <summary>
        ///     Async saving into files as binary
        /// </summary>
        /// <param name="app"></param>
        private void SaveVirtualFields(App app) {
            var alg = new Algorithms();
            alg.SaveVirtualFields(app);
        }

        #endregion

        #region Read

        /// <summary>
        ///     Reading from binary
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private App ReadVirtualFields(App app) {
            var alg = new Algorithms();
            return alg.ReadVirtualFields(app);
        }

        #endregion

        #endregion

        #region Adding Necessary Fields to record

        private string FixScriptTag(string str) {
            if (!string.IsNullOrWhiteSpace(str)) {
                //|embed|object|frameset|frame|iframe|
                var regEx = @"</?(?i:script|meta|link|style)(.|\n|\s)*?>";
                return Regex.Replace(str, regEx, string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }
            return str;
        }


        /// <summary>
        ///     Fix youtube link
        ///     add user
        ///     defaults
        ///     fix script tag
        ///     fix url friendly name
        ///     save virtual fields to text as async.
        ///     Don't manage tags ... it should be managed after saving to database.
        /// </summary>
        /// <param name="app"></param>
        private void AddingNeccessaryWhilePosting(App app) {
            app.ReviewsCount = 0;
            app.IsBlocked = false;
            app.TotalViewed = 0;
            app.WebsiteClicked = 0;
            app.StoreClicked = 0;
            app.CreatedDate = DateTime.Now;

            // Set userid, last mod, save virtual fields, fix iframe, URL
            AddNecessaryBothOnPostingNEditing(app);
        }

        /// <summary>
        ///     Set userid
        ///     save virtual fields
        ///     Last modified set
        ///     fix iframe
        ///     URL
        /// </summary>
        private void AddNecessaryWhenModified(App app) {
            // Set userid, last mod, save virtual fields, fix iframe, URL
            AddNecessaryBothOnPostingNEditing(app);
            _algorithms.RemoveSingleAppFromCacheOfStatic(app, db);
        }

        /// <summary>
        ///     Set userid
        ///     save virtual fields
        ///     Last modified set
        ///     fix iframe
        ///     URL
        /// </summary>
        /// <param name="app"></param>
        private void AddNecessaryBothOnPostingNEditing(App app) {
            app.URL = _algorithms.GenerateURLValid(app.PlatformVersion, app.CategoryID, app.AppName, app.PlatformID, db,
                app.AppID);
            app.UrlWithoutEscapseSequence = _algorithms.GetUrlStringExceptEscapeSequence(app.URL);
            app.PostedByUserID = UserManager.GetLoggedUserId();
            SaveVirtualFields(app);
            app.LastModifiedDate = DateTime.Now;
            app.AbsUrl = null;
        }

        #endregion

        #region DropDowns Generate

        public void GetDropDowns(App app = null) {
            if (app != null) {
                ViewBag.CategoryID = new SelectList(db.Categories.ToList(), "CategoryID", "CategoryName", app.CategoryID);
                ViewBag.PlatformID = new SelectList(db.Platforms.ToList(), "PlatformID", "PlatformName", app.PlatformID);
            } else {
                ViewBag.CategoryID = new SelectList(db.Categories.ToList(), "CategoryID", "CategoryName");
                ViewBag.PlatformID = new SelectList(db.Platforms.ToList(), "PlatformID", "PlatformName");
            }
        }

        public void GetDropDowns(Int64 id) {
            ViewBag.CategoryID = new SelectList(db.Categories.ToList(), "CategoryID", "CategoryName");
            ViewBag.PlatformID = new SelectList(db.Platforms.ToList(), "PlatformID", "PlatformName");
        }

        #endregion

        #region Remove Temporary and Draft from database when posting an app

        public void RemoveDraftRelatedtoApp(Guid uploadGuid) {
            new Thread(() => {
                using (var db2 = new WereViewAppEntities()) {
                    db2.Database.ExecuteSqlCommand("DELETE FROM AppDraft WHERE UploadGuid = @p0", uploadGuid);
                }
            }).Start();
        }

        public void RemoveTempUploadRelatedtoApp(Guid uploadGuid) {
            new Thread(() => {
                using (var db2 = new WereViewAppEntities()) {
                    db2.Database.ExecuteSqlCommand("DELETE FROM TempUpload WHERE RelatingUploadGuidForDelete = @p0",
                        uploadGuid);
                }
            }).Start();
        }

        public void RemoveTempUploadAndDraftFromRelatedApp(Guid uploadGuid) {
            //new Thread(() => {
            //    using (var db2 = new WereViewAppEntities()) {
            db.Database.ExecuteSqlCommand("DELETE FROM AppDraft WHERE UploadGuid = @p0", uploadGuid);
            db.Database.ExecuteSqlCommand("DELETE FROM TempUpload WHERE RelatingUploadGuidForDelete = @p0", uploadGuid);
            //    }
            //}).Start();
        }

        #endregion

        #region Post new app

        public ActionResult Post() {
            GetDropDowns();
            var app = new App {
                UploadGuid = Guid.NewGuid(),
                ReleaseDate = DateTime.Now
            };
            var viewOf = ViewTapping(ViewStates.Create);
            return View(app);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Post(App app) {
            var viewOf = ViewTapping(ViewStates.CreatePost, app);
            var currentApp = db.Apps.FirstOrDefault(n => n.UploadGuid == app.UploadGuid);
            GetDropDowns(app);
            if (currentApp != null) {
                // already saved.. 
                // then don't save it anymore.
                app.UploadGuid = Guid.NewGuid(); // new post can be made
                ModelState.Clear();
                return View(app);
            }

            AddingNeccessaryWhilePosting(app);
            if (ModelState.IsValid) {
                db.Apps.Add(app);
                var state = SaveDatabase(ViewStates.Create, app);
                if (state) {
                    ManageTagsInDatabase(app.AppID, app.UploadGuid, app.Tags);
                    RemoveTempUploadAndDraftFromRelatedApp(app.UploadGuid);
                    AppVar.SetSavedStatus(ViewBag, PostedSuccessFully(app.AppName, app.IsPublished));
                        // Saved Successfully.
                    app.UploadGuid = Guid.NewGuid(); // new post can be made
                    //app.AppName = app.AppName + " 2";
                    ModelState.Clear();
                    return View(app);
                }
            }
            // if you are here that means you have done the uploads
            // no need to give upload options anymore.
            ViewBag.UploadDontNeed = true;
            AppVar.SetErrorStatus(ViewBag, PostedFailed(app.AppName)); // Failed to Save
            return View(app);
        }

        #endregion

        #region Uploading files

        /// <summary>
        ///     starts with 1, get optimized sequence number.
        ///     Keep next sequence in session.
        /// </summary>
        /// <param name="uploadGuid"></param>
        /// <returns>Returns next sequence number. No need to do ++</returns>
        private byte GetSequenceNumber(Guid uploadGuid) {
            if (uploadGuid == null) {
                return 0;
            }
            var sessionCount = Session[uploadGuid.ToString()];
            Session[uploadGuid + "-staticCount"] = null;
            if (sessionCount != null) {
                var b = byte.Parse(sessionCount.ToString());
                Session[uploadGuid.ToString()] = b + 1;
                return b;
            }
            //try to get from the database.
            var max = 0;
            if (
                db.Galleries.Any(
                    n => n.UploadGuid == uploadGuid && n.GalleryCategoryID == GalleryCategoryIDs.AppPageGallery)) {
                max =
                    db.Galleries.Where(
                        n => n.UploadGuid == uploadGuid && n.GalleryCategoryID == GalleryCategoryIDs.AppPageGallery)
                        .Max(n => n.Sequence);
            }

            max += 2;
            Session[uploadGuid.ToString()] = max;
            max -= 1;
            return (byte) max;
        }

        private int GetHowManyGalleryImageExist(Guid uploadGuid) {
            if (uploadGuid == null) {
                return 0;
            }
            var sessionName = uploadGuid + "-count";
            Session[uploadGuid + "-staticCount"] = null;
            var sessionCount = Session[sessionName];
            if (sessionCount != null) {
                var b = byte.Parse(sessionCount.ToString());
                Session[sessionName] = b + 1;
                return b;
            }
            //try to get from the database.
            var max = 0;

            max =
                db.Galleries.Count(
                    n => n.UploadGuid == uploadGuid && n.GalleryCategoryID == GalleryCategoryIDs.AppPageGallery);


            max += 2;
            Session[sessionName] = max;
            max -= 1;
            return max;
        }

        private void ResetSessionForUploadSequence(Guid uploadGuid) {
            Session[uploadGuid.ToString()] = null;
            Session[uploadGuid + "-count"] = null;
            Session[uploadGuid + "-staticCount"] = null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UploadGallery(App app, IEnumerable<HttpPostedFileBase> galleries) {
            if (galleries != null && app.UploadGuid != null) {
                //look for cache if sequence exist before
                var nextSequence = GetSequenceNumber(app.UploadGuid);
                var nextCount = GetHowManyGalleryImageExist(app.UploadGuid);

                if (nextCount > AppVar.Setting.GalleryMaxPictures) {
                    ResetSessionForUploadSequence(app.UploadGuid);
                    return Json(new {isUploaded = false, uploadedFiles = 0, message = "You are out of your limit."},
                        "text/html");
                }
                var fileName = app.UploadGuid.ToString();
                var firstTime = true;
                var countDone = 0;
                foreach (var file in galleries) {
                    if (file.ContentLength > 0) {
                        // first save gallery and temp record.

                        if (!firstTime) {
                            nextSequence = GetSequenceNumber(app.UploadGuid);
                            nextCount = GetHowManyGalleryImageExist(app.UploadGuid);
                            if (nextCount > AppVar.Setting.GalleryMaxPictures) {
                                ResetSessionForUploadSequence(app.UploadGuid);
                                return
                                    Json(
                                        new {
                                            isUploaded = true,
                                            uploadedFiles = countDone,
                                            message = "You are out of your limit."
                                        }, "text/html");
                            }
                        } else {
                            firstTime = false;
                        }

                        //upload app-details page gallery image
                        WereViewStatics.uProcessorGallery.UploadFile(file, fileName, nextSequence, true, true);


                        //successfully uploaded now save a gallery info
                        var galleryCategory = await db.GalleryCategories.FindAsync(GalleryCategoryIDs.AppPageGallery);
                        //var thumbsCategory = await db.GalleryCategories.FindAsync(GalleryCategoryIDs.GalleryIcon);
                        var gallery =
                            await
                                db.Galleries.FirstOrDefaultAsync(
                                    n =>
                                        n.UploadGuid == app.UploadGuid &&
                                        n.GalleryCategoryID == galleryCategory.GalleryCategoryID &&
                                        n.Sequence == nextSequence);

                        // saving in the database
                        if (gallery == null) {
                            // we didn't get the error
                            // image sequence and guid is correct.
                            gallery = new Gallery();
                            gallery.GalleryID = Guid.NewGuid();
                            gallery.UploadGuid = app.UploadGuid;
                            if (!string.IsNullOrEmpty(app.AppName)) {
                                gallery.Title = app.AppName + "-" + nextSequence;
                            }
                            gallery.Extension = UploadProcessor.GetExtension(file);
                            gallery.GalleryCategoryID = galleryCategory.GalleryCategoryID;
                            gallery.Sequence = nextSequence;
                            db.Galleries.Add(gallery);
                            // try to add a temp record as well.
                            var tempUpload = new TempUpload();
                            tempUpload.TempUploadID = Guid.NewGuid();
                            tempUpload.UserID = UserManager.GetLoggedUserId();
                            tempUpload.GalleryID = gallery.GalleryID;
                            tempUpload.RelatingUploadGuidForDelete = app.UploadGuid;
                            db.TempUploads.Add(tempUpload);
                            await db.SaveChangesAsync();
                        }

                        // resize
                        //new Thread(() => {
                        
                        var source = "~/Uploads/Images/" + CommonVars.ADDITIONAL_ROOT_GALLERY_LOCATION +
                                     UploadProcessor.GetOrganizeNameStatic(gallery, true, true);
                        //checking if resize-source image already exist.
                        if (
                           System.IO.File.Exists(
                               WereViewStatics.uProcessorGallery.VirtualPathtoAbsoluteServerPath(source))) {
                            // if processed image exist then remove  the temp.
                            WereViewStatics.uProcessorGallery.RemoveTempImage(gallery);
                        }
                        // resize app-details page gallery image

                        WereViewStatics.uProcessorGallery.ProcessImage(gallery, galleryCategory);
                        //var source = "~/Uploads/Images/" + CommonVars.ADDITIONAL_ROOT_GALLERY_LOCATION +
                        //             UploadProcessor.GetOrganizeNameStatic(gallery, true, true);
                        //var target = "~/Uploads/Images/" + CommonVars.ADDITIONAL_ROOT_GALLERY_ICON_LOCATION +
                        //             UploadProcessor.GetOrganizeNameStatic(gallery, true);
                        
                        // #apps detail page gallery thumbs generate
                        //WereViewStatics.uProcessorGallery.ProcessImage(source, target, thumbsCategory.Width,
                        //    thumbsCategory.Height, gallery.Extension);
                       
                        countDone++;
                        //}).Start();
                    }
                }
                var countUploaded = galleries.Count();
                return
                    Json(
                        new {
                            isUploaded = true,
                            uploadedFiles = countUploaded,
                            message = "+" + countUploaded + " files successfully done."
                        }, "text/html");
            }
            return Json(new {isUploaded = false, uploadedFiles = 0, message = "No file send."}, "text/html");
        }

        #region Process Similar Uploads

        public JsonResult ProcessSingleUploads(App app, HttpPostedFileBase file, int galleryId,
            UploadProcessor uploadProcessorSepecific) {
            if (file != null && app.UploadGuid != null) {
                //look for cache if sequence exist before
                var fileName = app.UploadGuid.ToString();
                if (file.ContentLength > 0) {
                    // first save gallery and temp record.
                    //upload the image
                    uploadProcessorSepecific.UploadFile(file, fileName, 0, true, true);
                    //successfully uploaded now save a gallery info
                    var galleryCategory = db.GalleryCategories.Find(galleryId);
                    var gallery =
                        db.Galleries.FirstOrDefault(
                            n =>
                                n.UploadGuid == app.UploadGuid &&
                                n.GalleryCategoryID == galleryCategory.GalleryCategoryID);
                    if (gallery == null) {
                        gallery = new Gallery();
                        gallery.GalleryID = Guid.NewGuid();
                        gallery.UploadGuid = app.UploadGuid;
                        gallery.Title = app.AppName;
                        gallery.Extension = UploadProcessor.GetExtension(file);
                        gallery.GalleryCategoryID = galleryCategory.GalleryCategoryID;
                        gallery.Sequence = 0;
                        db.Galleries.Add(gallery);
                        // try to add a temp record as well.
                        var tempUpload = new TempUpload();
                        tempUpload.TempUploadID = Guid.NewGuid();
                        tempUpload.UserID = UserManager.GetLoggedUserId();
                        tempUpload.GalleryID = gallery.GalleryID;
                        tempUpload.RelatingUploadGuidForDelete = app.UploadGuid;
                        db.TempUploads.Add(tempUpload);
                        db.SaveChanges();
                    }
                    // resize
                    //new Thread(() => {

                    uploadProcessorSepecific.ProcessImage(gallery, galleryCategory);
                    uploadProcessorSepecific.RemoveTempImage(gallery);
                    //}).Start();
                }
                return Json(new {isUploaded = true, message = "successfully done"}, "text/html");
            }
            return Json(new {isUploaded = false, message = "No file send."}, "text/html");
        }

        #endregion

        #region Home Featured Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UploadHomeFeatured(App app, HttpPostedFileBase homePageFeatured) {
            return ProcessSingleUploads(app, homePageFeatured, GalleryCategoryIDs.HomePageFeatured,
                WereViewStatics.uProcessorHomeFeatured);
        }

        #endregion

        #region Home Page Icon Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UploadHomePageIcon(App app, HttpPostedFileBase homePageIcon) {
            return ProcessSingleUploads(app, homePageIcon, GalleryCategoryIDs.HomePageIcon,
                WereViewStatics.uProcessorHomeIcons);
        }

        #endregion

        #region Search Icon Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UploadSearchIcon(App app, HttpPostedFileBase searchIcon) {
            return ProcessSingleUploads(app, searchIcon, GalleryCategoryIDs.SearchIcon,
                WereViewStatics.uProcessorSearchIcons);
        }

        #endregion

        #region Suggestion Icon Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UploadSuggestionIcon(App app, HttpPostedFileBase suggestionIcon) {
            return ProcessSingleUploads(app, suggestionIcon, GalleryCategoryIDs.SuggestionIcon,
                WereViewStatics.uProcessorSuggestionIcons);
        }

        #endregion

        #endregion

        #region Edit or modify record

        public ActionResult Edit(Int64 id) {
            var userId = UserManager.GetLoggedUserId();
            var app = db.Apps.FirstOrDefault(n => n.AppID == id && n.PostedByUserID == userId);

            if (app == null) {
                return HttpNotFound();
            }
            var viewOf = ViewTapping(ViewStates.Edit, app);

            GetDropDowns(app); // Generating drop downs
            ReadVirtualFields(app);
            return View(app);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(App app) {
            var viewOf = ViewTapping(ViewStates.EditPost, app);
            if (ModelState.IsValid) {
                AddNecessaryWhenModified(app);
                db.Entry(app).State = EntityState.Modified;
                var state = SaveDatabase(ViewStates.Edit, app);
                if (state) {
                    ManageTagsInDatabase(app.AppID, app.UploadGuid, app.Tags);
                    AppVar.SetSavedStatus(ViewBag, EditSuccessFully(app.AppName, app.IsPublished));
                        // Saved Successfully.
                    return Redirect(app.GetAppUrl());
                }
            }

            GetDropDowns(app);
            AppVar.SetErrorStatus(ViewBag, EditedError); // Failed to save
            return View(app);
        }

        #endregion

        #region Delete or remove record

        //public ActionResult Delete(long id) {

        //    var app = db.Apps.Find(id);
        //    bool viewOf = ViewTapping(ViewStates.Delete, app);
        //    return View(app);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id) {
        //    var app = db.Apps.Find(id);
        //    bool viewOf = ViewTapping(ViewStates.DeletePost, app);
        //    db.Apps.Remove(app);
        //    bool state = SaveDatabase(ViewStates.Delete, app);
        //    if (!state) {
        //        AppVar.SetErrorStatus(ViewBag, _deletedError); // Failed to Save
        //        return View(app);
        //    }

        //    return RedirectToAction("Index");
        //}

        #endregion
    }
}