using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Models.EntityModel;
using WereViewApp.Modules.DevUser;

namespace WereViewApp.Controllers {
    public class ProfileController : Controller {
        // GET: Profile/username
        public ActionResult Index(string username, int? page) {
            var user = UserManager.GetUser(username);
            page = page ?? (page = 1);
            if (user != null) {
                using (var db = new WereViewAppEntities()) {
                    var apps = db.Apps
                        .Where(n => n.PostedByUserID == user.UserID)
                        .OrderByDescending(n => n.AppID);
                    long pages = 0;
                    long itemsInPage = AppConfig.Setting.PageItems;

                    apps = DevMVCComponent.Database.Pagination.GetPageData<App>(apps,pages,)               
                    return View(user);
                }
            }
            ViewBag.Reason = "User not found.";
            return View("_NotExist");
        }
    }
}