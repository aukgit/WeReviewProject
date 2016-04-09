using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeReviewApp.Controllers;
using WeReviewApp.Models.EntityModel;
using WeReviewApp.Models.EntityModel.ExtenededWithCustomMethods;
using WeReviewApp.Modules.DevUser;
using WeReviewApp.Modules.Extensions.IdentityExtension;

namespace WeReviewApp.Areas.Admin.Controllers {
    public class FeaturedImagesController : AdvanceController {
        #region Constructors

        public FeaturedImagesController()
            : base(true) {
            ViewBag.controller = _controllerName;
        }

        #endregion

        #region View tapping

        /// <summary>
        ///     Always tap once before going into the view.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="FeaturedImage">Gives the model if it is a editing state or creating posting state or when deleting.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        private bool ViewTapping(ViewStates view, FeaturedImage featuredImage = null) {
            switch (view) {
                case ViewStates.Index:
                    break;
                case ViewStates.Create:
                    break;
                case ViewStates.CreatePost: // before saving it
                    AddNecessaryFields(featuredImage);
                    break;
                case ViewStates.Edit:
                    break;
                case ViewStates.Details:
                    break;
                case ViewStates.EditPost: // before saving it
                    AddNecessaryFields(featuredImage);
                    break;
                case ViewStates.Delete:
                    break;
            }
            return true;
        }

        #endregion

        #region Add Necessary Fields

        private void AddNecessaryFields(FeaturedImage featuredImage = null) {
            if (featuredImage != null) {
                featuredImage.UserID = UserManager.GetLoggedUserId();
            }
        }

        #endregion

        #region Save database common method

        /// <summary>
        ///     Better approach to save things into database(than db.SaveChanges()) for this controller.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="FeaturedImage">Your model information to send in email to developer when failed to save.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        private bool SaveDatabase(ViewStates view, FeaturedImage featuredImage = null) {
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
                var changes = db.SaveChanges(featuredImage);
                if (changes > 0) {
                    return true;
                }
            } catch (Exception ex) {
                throw new Exception("Message : " + ex.Message + " Inner Message : " + ex.InnerException.Message);
            }
            return false;
        }

        #endregion

        #region DropDowns Generate

        public void GetDropDowns(FeaturedImage featuredImage = null) {
            var apps = db.Apps
                         .AsParallel()
                         .Select(n =>
                             new {
                                 n.AppID,
                                 AppName = n.GetAppUrlWithoutHostName()
                             }).ToList();

            if (featuredImage != null) {
                ViewBag.AppID = new SelectList(apps, "AppID", "AppName", featuredImage.AppID);
            } else {
                ViewBag.AppID = new SelectList(apps, "AppID", "AppName");
            }
        }

        #endregion

        #region Index

        public ActionResult Index() {
            var featuredImages = db.FeaturedImages.Include(f => f.App).Include(f => f.User);
            var viewOf = ViewTapping(ViewStates.Index);
            return View(featuredImages.ToList());
        }

        #endregion

        #region Details

        public ActionResult Details(long id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var featuredImage =
                db.FeaturedImages.Include(n => n.App).Include(n => n.User).FirstOrDefault(n => n.FeaturedImageID == id);
            if (featuredImage == null) {
                return HttpNotFound();
            }
            var viewOf = ViewTapping(ViewStates.Details, featuredImage);
            return View(featuredImage);
        }

        #endregion

        #region Removing output cache

        public void RemoveOutputCache(string url) {
            HttpResponse.RemoveOutputCacheItem(url);
        }

        #endregion

        #region Enums

        internal enum ViewStates {
            Index,
            Create,
            CreatePost,
            Edit,
            EditPost,
            Details,
            Delete,
            DeletePost
        }

        #endregion

        #region Developer Comments - Alim Ul karim

        // Generated by Alim Ul Karim on behalf of Developers Organism.
        // Find us developers-organism.com
        // https://www.facebook.com/DevelopersOrganism
        // mailto:alim@developers-organism.com		

        #endregion

        #region Constants

        private const string _deletedError =
            "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";

        private const string _deletedSaved = "Removed successfully.";
        private const string _editedSaved = "Modified successfully.";

        private const string _editedError =
            "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";

        private const string _createdError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        private const string _createdSaved = "Transaction is successfully added to the database.";
        private const string _controllerName = "FeaturedImages";

        /// Constant value for where the controller is actually visible.
        private const string _controllerVisibleUrl = "";

        #endregion

        #region Index Find - Commented

        /*
        public ActionResult Index(System.Int64 id) {
            var featuredImages = db.FeaturedImages.Include(f => f.App).Include(f => f.User).Where(n=> n. == id);
			bool viewOf = ViewTapping(ViewStates.Index);
            return View(featuredImages.ToList());
        }
		*/

        #endregion

        #region Create or Add

        public ActionResult Create() {
            GetDropDowns();
            var viewOf = ViewTapping(ViewStates.Create);
            return View();
        }

        /*
        public ActionResult Create(System.Int64 id) {        
            GetDropDowns(id); // Generate hidden.
            bool viewOf = ViewTapping(ViewStates.Create);
            return View();
        }
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeaturedImage featuredImage) {
            var viewOf = ViewTapping(ViewStates.CreatePost, featuredImage);
            GetDropDowns(featuredImage);
            if (db.FeaturedImages.Any(n => n.AppID == featuredImage.AppID)) {
                AppVar.SetErrorStatus(ViewBag, "This app is already exist. Can't save."); // Saved Successfully.
                return View(featuredImage);
            }
            featuredImage.UserID = User.GetUserID();
            if (ModelState.IsValid) {
                db.FeaturedImages.Add(featuredImage);
                var state = SaveDatabase(ViewStates.Create, featuredImage);
                if (state) {
                    AppVar.SetSavedStatus(ViewBag, _createdSaved); // Saved Successfully.
                } else {
                    AppVar.SetErrorStatus(ViewBag, _createdError); // Failed to save
                }

                return View(featuredImage);
            }
            AppVar.SetErrorStatus(ViewBag, _createdError); // Failed to Save
            return View(featuredImage);
        }

        #endregion

        #region Edit or modify record

        public ActionResult Edit(long id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var featuredImage = db.FeaturedImages.Find(id);
            if (featuredImage == null) {
                return HttpNotFound();
            }
            var viewOf = ViewTapping(ViewStates.Edit, featuredImage);
            GetDropDowns(featuredImage); // Generating drop downs
            return View(featuredImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FeaturedImage featuredImage) {
            var viewOf = ViewTapping(ViewStates.EditPost, featuredImage);
            if (ModelState.IsValid) {
                featuredImage.UserID = User.GetUserID();
                db.Entry(featuredImage).State = EntityState.Modified;
                var state = SaveDatabase(ViewStates.Edit, featuredImage);
                if (state) {
                    AppVar.SetSavedStatus(ViewBag, _editedSaved); // Saved Successfully.
                } else {
                    AppVar.SetErrorStatus(ViewBag, _editedError); // Failed to Save
                }

                return RedirectToAction("Index");
            }

            GetDropDowns(featuredImage);
            AppVar.SetErrorStatus(ViewBag, _editedError); // Failed to save
            return View(featuredImage);
        }

        #endregion

        #region Delete or remove record

        public ActionResult Delete(long id) {
            var featuredImage =
                db.FeaturedImages.Include(n => n.App).Include(n => n.User).FirstOrDefault(n => n.FeaturedImageID == id);
            var viewOf = ViewTapping(ViewStates.Delete, featuredImage);
            return View(featuredImage);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id) {
            var featuredImage =
                db.FeaturedImages.Include(n => n.App).Include(n => n.User).FirstOrDefault(n => n.FeaturedImageID == id);
            var viewOf = ViewTapping(ViewStates.DeletePost, featuredImage);
            db.FeaturedImages.Remove(featuredImage);
            var state = SaveDatabase(ViewStates.Delete, featuredImage);
            if (!state) {
                AppVar.SetErrorStatus(ViewBag, _deletedError); // Failed to Save
                return View(featuredImage);
            }

            return RedirectToAction("Index");
        }

        #endregion
    }
}