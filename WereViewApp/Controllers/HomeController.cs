#region using block

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevMvcComponent.EntityConversion;
using WeReviewApp.Models.POCO.IdentityCustomization;
using WeReviewApp.Models.POCO.Structs;
using WeReviewApp.Modules.DevUser;

#endregion

namespace WeReviewApp.Controllers {
    public class HomeController : BasicController {
        public HomeController()
            : base(true) {
            ViewBag.visibleUrl = ControllerVisibleUrl;
            ViewBag.dropDownDynamic = DropDownDynamic;
            ViewBag.dynamicLoadPartialController = DynamicLoadPartialController;
        }

        [OutputCache(CacheProfile = "Hour", VaryByCustom = "byuser")]
        public ActionResult Index() {
            return View();
        }

        [OutputCache(CacheProfile = "Year", VaryByCustom = "none")]
        public ActionResult About() {
            return View();
        }

        [OutputCache(CacheProfile = "Year", VaryByCustom = "none")]
        public ActionResult Policy() {
            return View();
        }

        //[OutputCache(Duration=84731)]
        //[OutputCache(CacheProfile = "Hour", VaryByCustom = "byuser")]
        [Authorize]
        public ActionResult ContactUs() {
            ViewBag.FeedbackCateoryID =
                new SelectList(
                    db.FeedbackCategories.Where(
                        n =>
                            !(n.FeedbackCategoryID == FeedbackCategoryIDs.MobileAppReport ||
                              n.FeedbackCategoryID == FeedbackCategoryIDs.ReviewReport)).ToList(), "FeedbackCategoryID",
                    "Category");
            AppVar.GetTitlePageMeta(ViewBag, "Contact Us", null, "Contact Us - " + AppVar.Name,
                "Contact Us, Feedback about " + AppVar.Name);
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<ViewResult> ContactUs(Feedback feedback) {
            AppVar.GetTitlePageMeta(ViewBag, "Contact Us", null, "Contact Us - " + AppVar.Name,
                "Contact Us, Feedback about " + AppVar.Name);
            var user = UserManager.GetCurrentUser();
            var oneHourBefore = DateTime.Now.AddHours(-1);
            var isReportedBefore = db.Feedbacks.Any(n => n.Username == user.UserName && n.PostedDate >= oneHourBefore);
            if (isReportedBefore) {
                return View("Later");
            }
            if (ModelState.IsValid) {
                feedback.PostedDate = DateTime.Now;
                feedback.Username = user.UserName;
                feedback.Name = user.DisplayName;
                db.Entry(feedback).State = EntityState.Added;
                db.SaveChanges();
                AppVar.SetSavedStatus(ViewBag);
                //send a email.
                var body = EntityToString.Get(feedback);
                AppVar.Mailer.NotifyAdmin("A feedback has been added by " + feedback.Email,
                    "Please check your feedback inbox.<br><br> Feedback :<br><q>" + feedback.Message + "</q><br>" + body);

                return View("Done");
            }

            ViewBag.FeedbackCateoryID =
                new SelectList(
                    db.FeedbackCategories.Where(
                        n =>
                            !(n.FeedbackCategoryID == FeedbackCategoryIDs.MobileAppReport ||
                              n.FeedbackCategoryID == FeedbackCategoryIDs.ReviewReport)).ToList(), "FeedbackCategoryID",
                    "Category");
            AppVar.SetErrorStatus(ViewBag);
            return View(feedback);
        }

        #region Constants and variables

        //const string DeletedError = "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";
        //const string DeletedSaved = "Removed successfully.";
        //const string EditedSaved = "Modified successfully.";
        //const string EditedError = "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";
        //const string CreatedError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        //const string CreatedSaved = "Transaction is successfully added to the database.";
        private const string ControllerName = "Home";

        /// Constant value for where the controller is actually visible.
        private const string ControllerVisibleUrl = "/Report/";

        //const string CurrentControllerRemoveOutputCacheUrl = "/Partials/ReportID";
        private const string DynamicLoadPartialController = "/Partials/";
        private readonly bool DropDownDynamic = true;

        #endregion
    }
}