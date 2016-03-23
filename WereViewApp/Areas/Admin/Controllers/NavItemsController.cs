using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DevMvcComponent;
using WeReviewApp.Controllers;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.POCO.IdentityCustomization;

namespace WeReviewApp.Areas.Admin.Controllers {
    public class NavItemsController : IdentityController<ApplicationDbContext> {
        public NavItemsController()
            : base(true) {

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
                return RedirectToAction("List", new { id = navigationItem.NavigationID });
            }
            AddMenuName(navigationItem.NavigationID);
            AppConfig.Caches.RemoveAllFromCache();
            return View(navigationItem);
        }

        public JsonResult SaveOrder(NavigationItem [] navigationItems) {
            NavigationItem dbNavigationItem = null;
            var len = navigationItems.Length;
            List<string> navigationItemsNames = new List<string>(len),
                         navigationItemsFailedNames = new List<string>(len); ;
            bool isFailed = false;
            foreach (var navItem in navigationItems) {
                try {
                    dbNavigationItem = db.NavigationItems.Find(navItem.NavigationItemID);
                    db.Entry(dbNavigationItem).State = EntityState.Modified;
                    dbNavigationItem.Ordering = navItem.Ordering;
                    if (db.SaveChanges() > -1) {
                        navigationItemsNames.Add(dbNavigationItem.Title);
                    }
                } catch (Exception ex) {
                    Mvc.Error.ByEmail(ex, "SaveOrder()", "", dbNavigationItem);
                    isFailed = true;
                    navigationItemsFailedNames.Add(dbNavigationItem.Title);
                }
            }
            if (isFailed) {
                return Json(new { success = !isFailed, titles = navigationItemsFailedNames }, JsonRequestBehavior.AllowGet);
            } else {
                return Json(new { success = !isFailed, titles = navigationItemsNames }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int id, int NavigationID) {
            var navigationItem = db.NavigationItems.Find(id);
            db.NavigationItems.Remove(navigationItem);
            db.SaveChanges();
            AddMenuName(NavigationID);
            AppConfig.Caches.RemoveAllFromCache();
            return RedirectToAction("List", new { id = NavigationID });
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}