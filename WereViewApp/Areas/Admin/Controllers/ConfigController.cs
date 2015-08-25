using System;
using System.Data.Entity;
using System.Web.Mvc;
using DevMvcComponent;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.IdentityCustomization;

namespace WereViewApp.Areas.Admin.Controllers {
    public class ConfigController : Controller {
        private readonly DevIdentityDbContext db = new DevIdentityDbContext();

        public ActionResult Index() {
            byte id = (byte)1;

            CoreSetting coreSetting = db.CoreSettings.Find(id);
            if (coreSetting == null) {
                return HttpNotFound();
            }
            return View(coreSetting);
        }

        public ActionResult SendEmail(string tab) {
            ViewBag.tab = "#email-setup";
            Mvc.Mailer.QuickSend(AppVar.Setting.DeveloperEmail, "Test Email", "Test Email at " + DateTime.Now.ToString());
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
                db.SaveChanges();
                AppConfig.RefreshSetting();
                ViewBag.Success = "Saved Successfully.";
            }

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
