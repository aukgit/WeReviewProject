#region using block

using System.Web.Mvc;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.IdentityCustomization;
using WereViewApp.Modules.Role;
using System.Linq;
using WereViewApp.Modules.DevUser;
using System.Data.Entity;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.POCO.Structs;
using WereViewApp.Modules.Session;

#endregion

namespace WereViewApp.Controllers {

    [Authorize]
    public class ReportController : AdvanceController {
        #region Constants and variables

        const string DeletedError = "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";
        const string DeletedSaved = "Removed successfully.";
        const string EditedSaved = "Modified successfully.";
        const string EditedError = "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";
        const string CreatedError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        const string CreatedSaved = "Transaction is successfully added to the database.";
        const string ControllerName = "Report";
        ///Constant value for where the controller is actually visible.
        const string ControllerVisibleUrl = "/Report/";
        const string CurrentControllerRemoveOutputCacheUrl = "/Partials/GetReportID";
        const string DynamicLoadPartialController = "/Partials/";
        bool DropDownDynamic = true;


        #endregion

        #region Application db

        private ApplicationDbContext db2 = new ApplicationDbContext();

        #endregion

        public ReportController()
            : base(true) {
            ViewBag.visibleUrl = ControllerVisibleUrl;
            ViewBag.dropDownDynamic = DropDownDynamic;
            ViewBag.dynamicLoadPartialController = DynamicLoadPartialController;
        }


        public void SetDefaults() {

        }
        private bool IsAppAlreadyReported(long appId, out App app) {
            string sessionAlreadyReported = "Report/AppIsAlreadyReported-" + appId;
            string sessionApp = "Report/ReportingApp-" + appId;

            if (Session[sessionAlreadyReported] == null) {
                app = db.Apps.Find(appId);
                var username = UserManager.GetCurrentUserName();
                var alreadyReported = db2.Feedbacks
                                             .Include(n => n.FeedbackAppReviewRelations)
                                             .Any(n => n.Username == username &&
                                                       n.IsInProcess &&
                                                       n.FeedbackAppReviewRelations
                                                        .Any(rel => rel.HasAppId && rel.AppID == appId));
                Session[sessionAlreadyReported] = alreadyReported;
                Session[sessionApp] = app;
            }
            app = (App)Session[sessionApp];
            return (bool)Session[sessionAlreadyReported];
        }

        private bool IsReviewAlreadyReported(long reviewId, out Review review) {
            string sessionAlreadyReported = "Report/ReviewIsAlreadyReported-" + reviewId;
            string sessionReview = "Report/ReportingReview-" + reviewId;

            if (Session[sessionAlreadyReported] == null) {
                review = db.Reviews.Find(reviewId);
                var username = UserManager.GetCurrentUserName();
                var alreadyReported = db2.Feedbacks
                                             .Include(n => n.FeedbackAppReviewRelations)
                                             .Any(n => n.Username == username &&
                                                       n.IsInProcess &&
                                                       n.FeedbackAppReviewRelations
                                                        .Any(rel => !rel.HasAppId && rel.ReviewID == reviewId));
                Session[sessionAlreadyReported] = alreadyReported;
                Session[sessionReview] = review;
            }
            review = (Review)Session[sessionReview];
            return (bool)Session[sessionAlreadyReported];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">AppId</param>
        /// <returns></returns>
        public ActionResult App(long id) {
            if (SessionNames.IsValidationExceed("App-Review")) {
                return View("Later");
            }
            if (RoleManager.IsInRole(RoleNames.Rookie) == false) {
                return AppVar.GetAuthenticationError("Unauthorized", "");
            }
            // if the app is already reported.
            App app;
            var isAlreadyReported = IsAppAlreadyReported(id, out app);
            if (app != null) {
                if (isAlreadyReported) {
                    ViewBag.isAppReport = true; // if the app is already reported
                    return View("AlreadyReported");
                } else {
                    ViewBag.id = id;
                    ViewBag.app = app;
                    return View();
                }
            }
            return View("_404");
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult App(Feedback feedback, long appOrReviewId, bool hasAppId) {
            if (SessionNames.IsValidationExceed("App-Review")) {
                return View("Later");
            }
            if (RoleManager.IsInRole(RoleNames.Rookie) == false) {
                return AppVar.GetAuthenticationError("Unauthorized", "");
            }
            App app;
            var isAlreadyReported = IsAppAlreadyReported(appOrReviewId, out app);
            if (isAlreadyReported == false && app != null) {
                // app is not reported before by this user.
                // now post the report.

                feedback.FeedbackCategoryID = FeedbackCategoryIDs.MobileAppReport;
                db2.Feedbacks.Add(feedback);
                // add the relationship.
                var relation = GetNewRelationship(appOrReviewId, true);
                feedback.FeedbackAppReviewRelations.Add(relation);
                if (db2.SaveChanges() > -1) {
                    // successfully saved.
                    // send an email to the admin.
                    AppVar.Mailer.Send(AppVar.Setting.AdminEmail, "User reported an app.",
                        "Please login and check at the admin panel , an app has been reported.");
                    return View("Done");
                }
            }
            ViewBag.isAppReport = true; // if the app is already reported
            return View("AlreadyReported");
        }

        public ActionResult Review(long id) {
            if (SessionNames.IsValidationExceed("Report-Review")) {
                return View("Later");
            }
            if (RoleManager.IsInRole(RoleNames.Rookie) == false) {
                return AppVar.GetAuthenticationError("Unauthorized", "");
            }

            Review review;
            var isReportedAlready = IsReviewAlreadyReported(id, out review);
            if (isReportedAlready == false && review != null) {
                var app = db.Apps.Find(id);
                ViewBag.app = app;
                ViewBag.review = review;
                ViewBag.id = id;
                return View();
            } else if (isReportedAlready && review != null) {
                return View("AlreadyReported");
            }
            return View("_404");
        }

        private FeedbackAppReviewRelation GetNewRelationship(long id, bool isApp) {
            var relation = new FeedbackAppReviewRelation() {
                HasAppId = isApp,
            };
            if (isApp) {
                relation.AppID = id;
                relation.ReviewID = -1;
            } else {
                relation.AppID = -1;
                relation.ReviewID = id;
            }
            return relation;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Review(Feedback feedback, long appOrReviewId, bool hasAppId) {
            if (SessionNames.IsValidationExceed("Report-Review")) {
                return View("Later");
            }
            if (RoleManager.IsInRole(RoleNames.Rookie) == false) {
                return AppVar.GetAuthenticationError("Unauthorized", "");
            }
            Review review;
            var isReportedAlready = IsReviewAlreadyReported(appOrReviewId, out review);
            if (isReportedAlready == false && review != null) {
                // review is not reported before by this user.
                // now post the report.
                feedback.FeedbackCategoryID = FeedbackCategoryIDs.ReviewReport;

                db2.Feedbacks.Add(feedback);
                // add the relationship.
                var relation = GetNewRelationship(appOrReviewId, false);
                feedback.FeedbackAppReviewRelations.Add(relation);
                if (db2.SaveChanges() > -1) {
                    // successfully saved.
                    // send an email to the admin.
                    AppVar.Mailer.Send(AppVar.Setting.AdminEmail, "A user has reported a review.",
                        "Please login and check at the admin panel , a review has been reported.");
                    return View("Done");
                }
                return View();
            }
            return View("AlreadyReported");
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            db2.Dispose();
        }
    }
}