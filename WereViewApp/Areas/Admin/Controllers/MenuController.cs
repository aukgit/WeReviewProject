#region using block

using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using WereViewApp.Controllers;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.IdentityCustomization;

#endregion

namespace WereViewApp.Areas.Admin.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]

    public class MenuController : IdentityController<ApplicationDbContext> {
        public MenuController() : base(true){
                
        }
        public ActionResult Index() {
            return View(db.Navigations.Include(n => n.NavigationItems).ToList());
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Navigation navigation) {
            if (ModelState.IsValid) {
                db.Navigations.Add(navigation);
                db.SaveChanges();
                ViewBag.Success = "Saved Successfully";
                return View(navigation);
            }
            return View(navigation);
        }

        public ActionResult Edit(Int32 id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var navigation = db.Navigations.Find(id);
            if (navigation == null) {
                return HttpNotFound();
            }
            return View(navigation);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Navigation navigation) {
            if (ModelState.IsValid) {
                db.Entry(navigation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Success = "Saved Successfully";
                return RedirectToAction("Index");
            }
            return View(navigation);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            var navigation = db.Navigations.Find(id);
            db.Navigations.Remove(navigation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}