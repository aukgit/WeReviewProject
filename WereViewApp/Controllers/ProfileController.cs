using DevMVCComponent.Database;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.AlgorithmsWereViewApp;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Modules.Cache;
using WereViewApp.Modules.DevUser;

namespace WereViewApp.Controllers {
    public class ProfileController : Controller {
        private Algorithms algorithms = new Algorithms();

        // GET: Profile/username
        public ActionResult Index(string username, int page = 1) {
            if (!string.IsNullOrWhiteSpace(username)) {
                var user = UserManager.GetUser(username);
                if (user != null) {
                    using (var db = new WereViewAppEntities()) {
                        var apps = db.Apps
                            .Where(n => n.PostedByUserID == user.UserID)
                            .Include(n => n.User)
                            .OrderByDescending(n => n.AppID);

                        var pageInfo = new PaginationInfo() {
                            ItemsInPage = AppConfig.Setting.PageItems,
                            PageNumber = page
                        };
                        var appsForThisPage = apps.GetPageData (pageInfo, CacheNames.ProfilePaginationData, true).ToList();
                        algorithms.GetEmbedImagesWithApp(appsForThisPage, db, (int)pageInfo.ItemsInPage, GalleryCategoryIDs.SearchIcon);
                        ViewBag.Apps = appsForThisPage;
                        string eachUrl = "/Profile/" + user.UserName + "/@page";
                        ViewBag.paginationHtml = Pagination.GetList(pageInfo, eachUrl, "", maxNumbersOfPagesShow:1);
                        return View(user);
                    }
                }
            }
            ViewBag.Reason = "User not found.";
            return View("_NotExist");
        }
    }
}