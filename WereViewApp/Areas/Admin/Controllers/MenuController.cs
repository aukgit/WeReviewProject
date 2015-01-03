using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.IdentityCustomization;

namespace WereViewApp.Areas.Admin.Controllers {
    public class MenuController : Controller {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index() {
            return View(db.Navigations.Include(n => n.NavigationItems).ToList());
        }



        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Navigation navigation) {
            if (ModelState.IsValid) {
                db.Navigations.Add(navigation);
                db.SaveChanges();
                ViewBag.Success = "Saved Successfully";
                return View(navigation);
            }
            return View(navigation);
        }

        public ActionResult Edit(System.Int32 id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Navigation navigation = db.Navigations.Find(id);
            if (navigation == null) {
                return HttpNotFound();
            }
            return View(navigation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Navigation navigation) {
            if (ModelState.IsValid) {
                db.Entry(navigation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Success = "Saved Successfully";
                return RedirectToAction("Index");
            }
            return View(navigation);
        }


        public ActionResult Delete(System.Int32 id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Navigation navigation = db.Navigations.Find(id);
            if (navigation == null) {
                return HttpNotFound();
            }
            return View(navigation);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Navigation navigation = db.Navigations.Find(id);
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
