#region using block

using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DevMvcComponent.EntityConversion;
using WereViewApp.Models.POCO.IdentityCustomization;

#endregion

namespace WereViewApp.Controllers {
    public class HomeController : BasicController {
        #region Constants and variables

        //const string DeletedError = "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";
        //const string DeletedSaved = "Removed successfully.";
        //const string EditedSaved = "Modified successfully.";
        //const string EditedError = "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";
        //const string CreatedError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        //const string CreatedSaved = "Transaction is successfully added to the database.";
        const string ControllerName = "Home";
        ///Constant value for where the controller is actually visible.
        const string ControllerVisibleUrl = "/Report/";
        //const string CurrentControllerRemoveOutputCacheUrl = "/Partials/ReportID";
        const string DynamicLoadPartialController = "/Partials/";
        bool DropDownDynamic = true;
        #endregion
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

        //[OutputCache(Duration=84731)]
        [OutputCache(CacheProfile = "Hour", VaryByCustom = "byuser")]
        public ActionResult ContactUs() {
            AppVar.GetTitlePageMeta(ViewBag, "Contact Us", null, "Contact Us - " + AppVar.Name,
                "Contact Us, Feedback about " + AppVar.Name);
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(Feedback feedback) {
            ViewBag.FeedbackCateoryID = new SelectList(db.FeedbackCategories.ToList(), "FeedbackCategoryID", "Category");
            AppVar.GetTitlePageMeta(ViewBag, "Contact Us", null, "Contact Us - " + AppVar.Name,
                "Contact Us, Feedback about " + AppVar.Name);


            if (ModelState.IsValid) {
                db.Entry(feedback).State = EntityState.Added;
                db.SaveChanges();
                AppVar.SetSavedStatus(ViewBag);
                //send a email.
                var body = EntityToString.Get(feedback);
                AppVar.Mailer.NotifyAdmin("A feedback has been added by " + feedback.Email,
                    "Please check your feedback inbox. Feedback :<br>" + feedback.Message + "<br>" + body);

                return View(feedback);
            }

            AppVar.SetErrorStatus(ViewBag);
            return View(feedback);
        }
    }
}