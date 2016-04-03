using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WeReviewApp.BusinessLogics.Admin;
using WeReviewApp.Controllers;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.POCO.IdentityCustomization;

namespace WeReviewApp.Areas.Admin.Controllers {
    public class NavItemsController : IdentityController<ApplicationDbContext> {
        private NavigationLogics _navigationLogic;
        public NavItemsController()
            : base(true) {
            _navigationLogic = new NavigationLogics(db);
        }
        private List<NavigationItem> GetItems(int? NavitionID = null) {
            var navs = db.NavigationItems.OrderBy(n => n.Ordering).ThenByDescending(n => n.NavigationItemID);

            if (NavitionID == null) {
                return navs.ToList();
            } else {
                return navs.Where(n => n.NavigationID == NavitionID).ToList();
            }
        }
        /// <summary>
        /// Parent Navigation Id 
        /// </summary>
        /// <param name="navigationId"></param>
        private void AddMenuName(int navigationId) {
            var nav = db.Navigations.Find(navigationId);
            ViewBag.MenuName = nav.Name;
            ViewBag.NavigationID = navigationId;
        }
        /// <summary>
        /// Get navigation list items with view.
        /// </summary>
        /// <param name="navigationId"></param>
        /// <param name="getWholeView"></param>
        /// <returns></returns>
        private ActionResult GetListView(int navigationId, bool getWholeView = true) {
            AddMenuName(navigationId);
            var list = GetItems(navigationId);

            if (getWholeView) {
                return View("List", list);
            } else {
                return PartialView("List", list);
            }
        }
        /// <summary>
        /// Public call for MVC to get the list view for expected navigation item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult List(int id) {
           return GetListView(id);
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

        public ActionResult SaveOrder(NavigationItem[] navigationItems) {
            if (_navigationLogic.SaveOrder(navigationItems)) {
                AppConfig.Caches.RemoveAllFromCache();
                return GetListView(navigationItems[0].NavigationID, false);
            }
            return HttpNotFound();
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