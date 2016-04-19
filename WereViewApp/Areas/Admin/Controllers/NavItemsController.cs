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
	
		private readonly NavigationLogics _navigationLogic;
        public NavItemsController()
            : base(true) {
            _navigationLogic = new NavigationLogics(db);
        }

        private List<NavigationItem> GetItems(int? NavitionID = null) {
            var navs = db.NavigationItems.OrderBy(n => n.Ordering).ThenByDescending(n => n.NavigationItemID);

            if (NavitionID == null) {
                return navs.ToList();
            }
            return navs.Where(n => n.NavigationID == NavitionID).ToList();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public ActionResult Edit(int id, int NavigationID) {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}