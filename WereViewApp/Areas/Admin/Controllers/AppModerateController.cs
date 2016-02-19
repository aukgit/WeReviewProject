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

        public AppModerateController():base(true) {
            
        }
        // GET: Admin/AppModerate
        public ActionResult Index(long id) {
            var appModerateModel = new AppModerateViewModel() {
                AppId = id
            };

            var app  = db.Apps.Find(id);
            if (app == null) {
                return HttpNotFound();
            }
            ViewBag.app = app;
            appModerateModel.App = app;
            appModerateModel.IsBlocked = app.IsBlocked;
            appModerateModel.IsFeatured = db.FeaturedImages.Any(n=> n.IsFeatured && n.AppID == id);
            return View(appModerateModel);
        }
        [HttpPost]
        public ActionResult Index(AppModerateViewModel model) {
            var app = ViewBag.app as App ;
            if (app != null) {
                if(app.is)
            }
        
        }
    }
}