using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WereViewApp.Controllers;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.IdentityCustomization;

namespace WereViewApp.Areas.Admin.Controllers {
    public class NavItemsController : IdentityController<ApplicationDbContext> {
        public NavItemsController():base(true) {
            
        }
        private List<NavigationItem> GetItems(int? NavitionID = null) {
            if (NavitionID == null) {
                return db.NavigationItems.ToList();
            } else {
                return db.NavigationItems.Where(n => n.NavigationID == NavitionID).ToList();
            }
        }

        private void AddMenuName(int id) {
            var nav = db.Navigations.Find(id);
            ViewBag.MenuName = nav.Name;
            ViewBag.NavigationID = id;
        }

        public ActionResult List(int id) {
            AddMenuName(id);
            return View(GetItems(id));
        }

        private void HasDropDownAttr(NavigationItem navigationItem) {
            if (!navigationItem.HasDropDown) {
                navigationItem.ParentNavigationID = null;
            }
        }

        public ActionResult Add(int id) {
            AddMenuName(id);
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(NavigationItem navigationItem) {
            AddMenuName(navigationItem.NavigationID);
            if (ModelState.IsValid) {
                HasDropDownAttr(navigationItem);
                db.NavigationItems.Add(navigationItem);
                AppVar.SetSavedStatus(ViewBag);
                db.SaveChanges();
                AppConfig.Caches.RemoveAllFromCache();
                return View(navigationItem);
            }
            AppVar.SetErrorStatus(ViewBag);
            return View(navigationItem);
        }

        public ActionResult Edit(Int32 id, int NavigationID) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Editing = true;
            var navigationItem = db.NavigationItems.Find(id);
            if (navigationItem == null) {
                return HttpNotFound();
            }
            AddMenuName(NavigationID);
            return View(navigationItem);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(NavigationItem navigationItem) {
            ViewBag.Editing = true;
            HasDropDownAttr(navigationItem);
            if (ModelState.IsValid) {
                db.Entry(navigationItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List", new {id = navigationItem.NavigationID});
            }
            AddMenuName(navigationItem.NavigationID);
            AppConfig.Caches.RemoveAllFromCache();
            return View(navigationItem);
        }

        public ActionResult Delete(int id, int NavigationID) {
            var navigationItem = db.NavigationItems.Find(id);
            db.NavigationItems.Remove(navigationItem);
            db.SaveChanges();
            AddMenuName(NavigationID);
            AppConfig.Caches.RemoveAllFromCache();
            return RedirectToAction("List", new {id = NavigationID});
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult SaveOrder(object id) {
            return null;
        }
    }
}