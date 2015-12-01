﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevTrends.MvcDonutCaching;
using WereViewApp.Controllers;
//using DevTrends.MvcDonutCaching;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.IdentityCustomization;
using DevMvcComponent.Pagination;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.ExtenededWithCustomMethods;
using WereViewApp.Models.POCO.Enum;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Mail;

namespace WereViewApp.Areas.Admin.Controllers {
    [OutputCache(NoStore = true)]

    public class FeedbacksController : IndentityController<ApplicationDbContext> {

        #region Developer Comments - Alim Ul karim
        /*
         *  Generated by Alim Ul Karim on behalf of Developers Organism.
         *  Find us developers-organism.com
         *  https://fb.com/DevelopersOrganism
         *  mailto:alim@developers-organism.com	
         *  Google 'https://www.google.com.bd/search?q=Alim-ul-karim'
         *  First Written : 23 March 2014
         *  Modified      : 03 March 2015
         * * */
        #endregion

        #region Constants and variables

        const string DeletedError = "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";
        const string DeletedSaved = "Removed successfully.";
        const string EditedSaved = "Modified successfully.";
        const string EditedError = "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";
        const string CreatedError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        const string CreatedSaved = "Transaction is successfully added to the database.";
        const string ControllerName = "Feedbacks";
        ///Constant value for where the controller is actually visible.
        const string ControllerVisibleUrl = "/Admin/Feedbacks/";
        const string CurrentControllerRemoveOutputCacheUrl = "/Partials/GetFeedbackID";
        const string DynamicLoadPartialController = "/Partials/";
        bool DropDownDynamic = true;
        #endregion

        #region Enums

        internal enum ViewStates {
            Index,
            Create,
            CreatePostBefore,
            CreatePostAfter,
            Edit,
            EditPostBefore,
            EditPostAfter,
            Details,
            Delete,
            DeletePost
        }

        #endregion

        #region Constructors

        public FeedbacksController()
            : base(true) {
            ViewBag.controller = ControllerName;
            ViewBag.visibleUrl = ControllerVisibleUrl;
            ViewBag.dropDownDynamic = DropDownDynamic;
            ViewBag.dynamicLoadPartialController = DynamicLoadPartialController;
        }

        #endregion

        #region View tapping
        /// <summary>
        /// Always tap once before going into the view.
        /// </summary>
        /// <param name="view">Say the view state, where it is calling from.</param>
        /// <param name="feedback">Gives the model if it is a editing state or creating posting state or when deleting.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        bool ViewTapping(ViewStates view, Feedback feedback = null, bool entityValidState = true) {
            switch (view) {
                case ViewStates.Index:
                    break;
                case ViewStates.Create:
                    break;
                case ViewStates.CreatePostBefore: // before saving it
                    break;
                case ViewStates.CreatePostAfter: // after saving
                    break;
                case ViewStates.Edit:
                    break;
                case ViewStates.Details:
                    break;
                case ViewStates.EditPostBefore: // before saving it
                    break;
                case ViewStates.EditPostAfter: // after saving
                    break;
                case ViewStates.Delete:
                    break;
            }
            return true;
        }
        #endregion

        #region Save database common method

        /// <summary>
        /// Better approach to save things into database(than db.SaveChanges()) for this controller.
        /// </summary>
        /// <param name="view">Say the view state, where it is calling from.</param>
        /// <param name="feedback">Your model information to send in email to developer when failed to save.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        bool SaveDatabase(ViewStates view, Feedback feedback = null) {
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
                var changes = db.SaveChanges(feedback);
                if (changes > 0) {
                    RemoveOutputCacheOnIndex();
                    RemoveOutputCache(CurrentControllerRemoveOutputCacheUrl);
                    return true;
                }
            } catch (Exception ex) {
                throw new Exception("Message : " + ex.Message + " Inner Message : " + ex.InnerException.Message);
            }
            return false;
        }
        #endregion

        #region DropDowns Generate

        #endregion

        #region Get Paged Feedbacks on Filter
        private ActionResult GetPagedFeedbacks(Expression<Func<Feedback, bool>> condition, string actionName, int? page = 1) {
            var paginationInfo = new PaginationInfo() {
                ItemsInPage = AppVar.Setting.PageItems,
                PageNumber = page,
                PagesExists = null
            };
            string cacheName = "admin.feedback." + actionName;
            IOrderedQueryable<Feedback> feedbacks;
            if (condition != null) {
                feedbacks = db.Feedbacks
                    .Where(condition)
                    .OrderByDescending(n => n.FeedbackID);
            } else {
                feedbacks = db.Feedbacks
                    .OrderByDescending(n => n.FeedbackID);
            }

            var pagedData = feedbacks.GetPageData(paginationInfo, cacheName: cacheName);

            var url = ControllerVisibleUrl + actionName + "/@page";
            ViewBag.paginationHtml = Pagination.GetList(paginationInfo, url, cacheName: cacheName + ".nav.html");
            return View("Index", pagedData);
        }
        #endregion

        #region Filters and Index
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Index(int? page) {
            bool viewOf = ViewTapping(ViewStates.Index);
            var action = System.Reflection.MethodBase.GetCurrentMethod().Name;

            return GetPagedFeedbacks(n => !n.IsViewed, action, page);
        }
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult IsViewed(int? page) {
            var action = System.Reflection.MethodBase.GetCurrentMethod().Name;
            return GetPagedFeedbacks(n => n.IsViewed && !n.IsInProcess && !n.IsUnSolved && !n.IsSolved, action, page);
        }

        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult UnSolved(int? page) {
            var action = System.Reflection.MethodBase.GetCurrentMethod().Name;
            return GetPagedFeedbacks(n => n.IsUnSolved, action, page);
        }

        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult IsInProcess(int? page) {
            var action = System.Reflection.MethodBase.GetCurrentMethod().Name;
            return GetPagedFeedbacks(n => n.IsInProcess, action, page);
        }

        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Solved(int? page) {
            var action = System.Reflection.MethodBase.GetCurrentMethod().Name;
            return GetPagedFeedbacks(n => n.IsSolved, action, page);
        }
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult AllFeedbacks(int? page) {
            var action = System.Reflection.MethodBase.GetCurrentMethod().Name;
            return GetPagedFeedbacks(null, action, page);
        }
        #endregion

        #region Index Find - Commented
        /*
        [OutputCache(CacheProfile = "Year")]
        public ActionResult Index(System.long id) {
			bool viewOf = ViewTapping(ViewStates.Index);
            return View(db.Feedbacks.Where(n=> n. == id).ToList());
        }
		*/
        #endregion

        #region Details
        public ActionResult Details(long id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feedback = db.Feedbacks.Find(id);
            if (feedback == null) {
                return HttpNotFound();
            }
            bool viewOf = ViewTapping(ViewStates.Details, feedback);
            return View(feedback);
        }
        #endregion


        #region Mark as view unview
        private Feedback MarkAsViewedInternal(Feedback feedback) {
            feedback.SetStatus(FeedbackStatusTypes.IsViewed);
            db.Entry(feedback).State = EntityState.Modified;
            SaveDatabase(ViewStates.Edit, feedback);
            return feedback;
        }

        private Feedback MarkAsNonViewedInternal(Feedback feedback) {
            feedback.SetStatus(statusType: FeedbackStatusTypes.IsNonViewed);
            db.Entry(feedback).State = EntityState.Modified;
            SaveDatabase(ViewStates.Edit, feedback);
            return feedback;
        }
        #endregion

        #region Replying
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Reply(long id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feedback = db.Feedbacks.Find(id);
            if (feedback == null) {
                return HttpNotFound();

            }
            if (!feedback.IsViewed) {
                // new feedback now mark as viewed
                feedback = MarkAsViewedInternal(feedback);
            }

            if (feedback.HasAppOrReviewReport) {
                var relation = db.FeedbackAppReviewRelations.FirstOrDefault(n => n.FeedbackID == feedback.FeedbackID);
                ViewBag.relation = relation;
                if (relation != null) {
                    using (var db2 = new WereViewAppEntities()) {
                        if (relation.HasAppId) {
                            ViewBag.app = db2.Apps.Find(relation.AppID);
                        } else {
                            var review = db2.Reviews.Find(relation.ReviewID);
                            if (review != null) {
                                ViewBag.app = db2.Apps.Find(review.AppID);
                                ViewBag.review = review;
                            }
                        }
                    }
                }
            }

            bool viewOf = ViewTapping(ViewStates.Edit, feedback);

            return View(feedback);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public async Task<ActionResult> Reply(Feedback feedback, string Status) {
            bool viewOf = ViewTapping(ViewStates.EditPostBefore, feedback);
            if (ModelState.IsValid) {

                // set status
                SetStatus(ref feedback, Status);
                db.Entry(feedback).State = EntityState.Modified;
                bool state = SaveDatabase(ViewStates.Edit, feedback);
                if (state) {
                    AppVar.SetSavedStatus(ViewBag, EditedSaved); // Saved Successfully.



                    // send email
                    SendEmail(feedback);

                } else {
                    AppVar.SetErrorStatus(ViewBag, EditedError); // Failed to Save
                }
                viewOf = ViewTapping(ViewStates.EditPostAfter, feedback, state);
                return RedirectToAction("Index");
            }
            viewOf = ViewTapping(ViewStates.EditPostAfter, feedback, false);
            AppVar.SetErrorStatus(ViewBag, EditedError); // record not valid for save
            return View(feedback);
        }
        #endregion

        #region Set Status
        private void SetStatus(ref Feedback feedback, string status) {
            switch (status) {
                case "in-process":
                    feedback.SetStatus(FeedbackStatusTypes.IsInProcess);
                    break;
                case "is-solved":
                    feedback.SetStatus(FeedbackStatusTypes.IsSolved);
                    break;

                case "is-unsolved":
                    feedback.SetStatus(FeedbackStatusTypes.IsUnsolved);
                    break;

            }
        }
        #endregion

        #region Mailing

        private async void SendEmail(Feedback feedback, string department = "Administrator") {
            var mailSender = new MailSender();
            var subject = GetSubject(feedback);
            var body = GetEmailBody(feedback, department);
            mailSender.Send(feedback.Email, subject, body);
        }
        private string GetEmailBody(Feedback feedback, string department = "Administrator") {
            var sb = new StringBuilder(100);
            var user = UserManager.GetUser(feedback.Username);
            MailHtml.AddGreetingsToStringBuilder(user, sb);
            sb.AppendLine("Status:");
            sb.AppendLine("<q><strong>");
            sb.AppendLine(feedback.GetStatus().Status);
            sb.AppendLine("</strong></q>");
            sb.AppendLine(MailHtml.LineBreak);
            sb.AppendLine(MailHtml.LineBreak);
            sb.AppendLine("In response to your:");
            sb.AppendLine("<q><strong>");
            sb.AppendLine(feedback.Message);
            sb.AppendLine("</strong></q>");
            sb.AppendLine(MailHtml.LineBreak);
            sb.AppendLine(MailHtml.LineBreak);
            sb.AppendLine("Our response:");
            sb.AppendLine("<q><strong>");
            sb.AppendLine(feedback.Response);
            sb.AppendLine("</strong></q>");
            sb.AppendLine(MailHtml.LineBreak);
            sb.AppendLine(MailHtml.LineBreak);
            MailHtml.AddContactUsToStringBuilder(sb);
            sb.AppendLine(MailHtml.LineBreak);
            sb.AppendLine(MailHtml.LineBreak);
            MailHtml.AddThanksFooterOnStringBuilder(AppVar.Setting.AdminName, department, sb);
            var mail = sb.ToString();
            sb = null;
            GC.Collect();
            return mail;
        }

        private string GetSubject(Feedback feedback) {
            return "RE : " + feedback.Subject;
        }
        #endregion

        #region Blocking

        public async Task<ActionResult> BlockApp(Int64 id) {
            using (var db2 = new WereViewAppEntities()) {
                var app = db2.Apps.Find(id);
                app.IsBlocked = true;
                app.Tags = "none";
                if (db2.SaveChanges() > -1) {
                    var user = UserManager.GetUser(app.PostedByUserID);
                    var mailer = new MailSender();
                    mailer.Send(user.Email, "Your app has been blocked.",
                        "Sorry! Your app <a href='" + app.GetAbsoluteUrl() + "'>" + app.AppName + "</a> is inappropriate thus blocked.");
                    ViewBag.info = "block the app.";
                    return View("Done");
                }
            }
            return HttpNotFound();
        }
        public async Task<ActionResult> UnBlockApp(Int64 id) {
            using (var db2 = new WereViewAppEntities()) {
                var app = db2.Apps.Find(id);
                app.IsBlocked = false;
                app.Tags = "none";
                if (db2.SaveChanges() > -1) {
                    var user = UserManager.GetUser(app.PostedByUserID);
                    var mailer = new MailSender();
                    mailer.Send(user.Email, "Your app has been unblocked.",
                        "Congratulations! Your app <a href='" + app.GetAbsoluteUrl() + "'>" + app.AppName + "</a> is now unblocked.");
                    ViewBag.info = "unblock the app.";
                    return View("Done");
                }
            }
            return HttpNotFound();
        }

        public async Task<ActionResult> BlockReview(Int64 id) {
            using (var db2 = new WereViewAppEntities()) {
                var review = db2.Reviews.Find(id);
                var likeDislikes = db2.ReviewLikeDislikes.Where(n => n.ReviewID == id);
                foreach (var likeDislike in likeDislikes) {
                    db2.ReviewLikeDislikes.Remove(likeDislike);
                }
                ViewBag.info = "deleted review ( " + review.Comments + " ).";
                db2.Reviews.Remove(review);
                if (db2.SaveChanges() > -1) {
                    var user = UserManager.GetUser(review.UserID);
                    var mailer = new MailSender();
                    mailer.Send(user.Email, "Your review has been removed.",
                        "Sorry! Your review <q>" + review.Comments + "</q> is inappropriate thus removed.");

                    return View("Done");
                }
            }

            return HttpNotFound();
        }
        #endregion


        #region Removing output cache
        public void RemoveOutputCache(string url) {
            HttpResponse.RemoveOutputCacheItem(url);
        }

        public void RemoveOutputCacheOnIndex() {
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems(ControllerName, "Index");
            cacheManager.RemoveItems(ControllerName, "List");
            RemoveOutputCache(ControllerVisibleUrl);
            RemoveOutputCache(ControllerVisibleUrl + "Index");
            cacheManager = null;
            GC.Collect();
        }
        #endregion
    }


}
