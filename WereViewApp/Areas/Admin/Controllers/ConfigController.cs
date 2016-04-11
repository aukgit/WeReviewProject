using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using WeReviewApp.BusinessLogics;
using WeReviewApp.Filter;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.POCO.IdentityCustomization;
using WeReviewApp.Modules.Role;

namespace WeReviewApp.Areas.Admin.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public class ConfigController : Controller {
        private readonly DevIdentityDbContext db = new DevIdentityDbContext();

        public ActionResult Index() {
            var id = (byte) 1;

            var coreSetting = db.CoreSettings.Find(id);
            if (coreSetting == null) {
                return HttpNotFound();
            }
            return View(coreSetting);
        }

        [Authorize]
        [CheckRegistrationComplete]
        public ActionResult CleanSystem() {
            return View();
        }

        [Authorize]
        [HasMinimumRole(MinimumRole = RoleNames.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CleanSystem(string clean) {
            if (!string.IsNullOrEmpty(clean) && clean.Equals("Clean")) {
                var algorithm = new Logics();
                ViewBag.message = "Every thing is removed successfully.";
                AppVar.SetErrorStatus("Sorry ! Some went wrong in the server. Please get in touch with developer.");
            }
            return View();
        }

        public async Task<ActionResult> SendEmail(string tab) {
            ViewBag.tab = "#email-setup";
            AppVar.Mailer.NotifyAdmin(AppVar.Setting.DeveloperEmail, "Test Email", "Test Email at " + DateTime.Now);
            try {
                throw new Exception("Testing error mail.");
            } catch (Exception ex) {
                AppVar.Mailer.HandleError(ex, "Test");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CoreSetting coreSetting, string tab) {
            ViewBag.tab = tab;

            if (ModelState.IsValid) {
                db.Entry(coreSetting).State = EntityState.Modified;
                if (db.SaveChanges() > -1) {
                    AppConfig.RefreshSetting();
                    AppVar.SetSavedStatus(ViewBag);
                    return View(coreSetting);
                }
            }
            AppVar.SetErrorStatus(ViewBag);
            return View(coreSetting);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}