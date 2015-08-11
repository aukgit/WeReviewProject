#region using block

using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DevMvcComponent.Pagination;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Modules.Cache;
using WereViewApp.Modules.DevUser;
using WereViewApp.WereViewAppCommon;

#endregion

namespace WereViewApp.Controllers {
    public class ProfileController : Controller {
        private const int MaxNumbersOfPagesShow = 8;

        [OutputCache(CacheProfile = "Day", VaryByParam = "page")]
        public ActionResult Index(int page = 1) {
            //var db2 = new ApplicationDbContext();
            ViewBag.Title = "Developers apps list with reviews";
            ViewBag.Meta = "Mobile apps, apps review, apple apps, android apps,reviews, app review site, " +
                           ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            var users = UserManager
                .GetAllUsersAsIQueryable();


            var cachePagesString = AppConfig.Caches.Get(CacheNames.ProfilePaginationDataCount);
            var count = -1;
            if (cachePagesString == null) {
                count = users.Select(n => n.Id).Count();
            } else {
                count = (int) cachePagesString;
            }
            // add ordered by
            users = users.OrderByDescending(n => n.Id);
            var pageInfo = new PaginationInfo {
                ItemsInPage = AppConfig.Setting.PageItems + 40,
                PageNumber = page,
                PagesExists = count
            };
            var usersForThisPage = users.GetPageData(pageInfo, CacheNames.ProfilePaginationDataCount, false).ToList();
            const string eachUrl = "/Profile?page=@page";
            ViewBag.paginationHtml = Pagination.GetList(pageInfo, eachUrl, "",
                maxNumbersOfPagesShow: MaxNumbersOfPagesShow);
            return View(usersForThisPage);
        }

        // GET: Profile/username
        [OutputCache(CacheProfile = "Day", VaryByParam = "username,page")]
        public ActionResult GetProfile(string username, int page = 1) {
            if (!string.IsNullOrWhiteSpace(username)) {
                var algorithms = new Algorithms();
                ViewBag.Title = username + "'s apps collection";
                ViewBag.Meta = "Mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
                ViewBag.Keywords = ViewBag.Meta;

                var user = UserManager.GetUser(username);
                if (user != null) {
                    using (var db = new WereViewAppEntities()) {
                        var apps = algorithms.GetViewableApps(db)
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
                        algorithms.GetEmbedImagesWithApp(appsForThisPage, db, (int) pageInfo.ItemsInPage,
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
            return View("_404");
        }
    }
}