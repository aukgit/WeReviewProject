using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Controllers;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.ViewModels;

namespace WereViewApp.Areas.Admin.Controllers {
    public class AppModerateController : AdvanceController {

        public AppModerateController()
            : base(true) {

        }

        private const string TempAppKey = "app-moderate";
        private const string TempAppFeaturedKey = "app-moderate-is-featured";

        // GET: Admin/AppModerate
        public ActionResult Index(long id) {
            var appModerateModel = new AppModerateViewModel() {
                AppId = id
            };

            var app = db.Apps.Find(id);
            if (app == null) {
                return HttpNotFound();
            }
            TempData[TempAppKey] = app;
            appModerateModel.App = app;
            appModerateModel.IsBlocked = app.IsBlocked;
            appModerateModel.IsFeatured = db.FeaturedImages.Any(n => n.IsFeatured && n.AppID == id);
            TempData[TempAppFeaturedKey] = appModerateModel.IsFeatured;
            return View(appModerateModel);
        }
        [HttpPost]
        public ActionResult Index(AppModerateViewModel model) {
              var app =         TempData[TempAppKey]  as App;
            var isFeaturedPreviously = (bool)TempData[TempAppFeaturedKey];

            if (app != null) {
                if (app.IsBlocked != model.IsBlocked) {
                    // needs to update
                }
                if (isFeaturedPreviously != model.IsFeatured) {
                    // needs to update
                }
            }
            AppVar.SetSavedStatus(this.ViewBag, "You have successfully moderated this app.");
            return View(model);
        }
    }
}