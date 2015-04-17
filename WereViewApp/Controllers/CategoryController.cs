using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index() {
            var alg = new Algorithms();
            var categories = alg.GetCategoryWiseAppsForCategoryPage();
            return View(categories);
        }

        public ActionResult Specific(string categoryName, int page = 1) {
            if (!string.IsNullOrWhiteSpace(categoryName)) {
                var alg = new Algorithms();
                var categories = alg.GetCategoryWiseAppsForCategoryPage();

                ViewBag.Title = username + "'s apps collection";
                ViewBag.Meta = "Mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
                ViewBag.Keywords = ViewBag.Meta;

                var user = UserManager.GetUser(username);
                if (user != null) {
                    using (var db = new WereViewAppEntities()) {
                        var apps = db.Apps
                            .Where(n => n.PostedByUserID == user.UserID)
                            .Include(n => n.User)
                            .OrderByDescending(n => n.AppID);

                        var pageInfo = new PaginationInfo {
                            ItemsInPage = AppConfig.Setting.PageItems,
                            PageNumber = page,
                            PagesExists = -1
                        };
                        var appsForThisPage =
                            apps.GetPageData(pageInfo, CacheNames.ProfilePaginationDataForSpecificProfile, true)
                                .ToList();
                        algorithms.GetEmbedImagesWithApp(appsForThisPage, db, (int)pageInfo.ItemsInPage,
                            GalleryCategoryIDs.SearchIcon);
                        ViewBag.Apps = appsForThisPage;
                        var eachUrl = "/Profile/" + user.UserName + "/@page";
                        ViewBag.paginationHtml = Pagination.GetList(pageInfo, eachUrl, "",
                            maxNumbersOfPagesShow: MaxNumbersOfPagesShow);
                        return View(user);
                    }
                }
            }
            ViewBag.Reason = "User not found.";
            return View("_NotExist");
            return View(categories);
        }
    }
}