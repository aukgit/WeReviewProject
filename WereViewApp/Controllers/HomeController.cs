#region using block

using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DevMVCComponent.Database;
using WereViewApp.Models.POCO.IdentityCustomization;

#endregion

namespace WereViewApp.Controllers {
    public class HomeController : BasicController {
        public HomeController()
            : base(true) {
        }

        [OutputCache(CacheProfile = "Hour", VaryByCustom = "byuser")]
        public ActionResult Index() {
            return View();
        }

        //[OutputCache(Duration=84731)]
        [OutputCache(CacheProfile = "Hour", VaryByCustom = "byuser")]
        public ActionResult ContactUs() {
            ViewBag.FeedbackCateoryID = new SelectList(db.FeedbackCategories.ToList(), "FeedbackCategoryID", "Category");
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
                var body = EntityToString.GetHTML(feedback);
                AppVar.Mailer.NotifyAdmin("A feedback has been added by " + feedback.Email,
                    "Please check your feedback inbox. Feedback :<br>" + feedback.Message + "<br>" + body);

                return View(feedback);
            }

            AppVar.SetErrorStatus(ViewBag);
            return View(feedback);
        }
    }
}