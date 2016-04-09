#region using block

using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevMvcComponent.Pagination;
using WeReviewApp.Models.EntityModel.Structs;
using WeReviewApp.Models.POCO.Identity;
using WeReviewApp.Modules.Cache;
using WeReviewApp.Modules.DevUser;
using WeReviewApp.WereViewAppCommon;

#endregion

namespace WeReviewApp.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public class ProfilesController : AdvanceController {

        public ProfilesController()
            : base(true) {

        }
        private const int MaxNumbersOfPagesShow = 8;

        //[OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        [OutputCache(CacheProfile = "Hour", VaryByParam = "*")]
        public ActionResult Index(int page = 1) {
            //var db2 = new ApplicationDbContext();
            ViewBag.Title = "Developers apps list with reviews";
            ViewBag.Meta = "Mobile apps, apps review, apple apps, android apps,reviews, app review site, " +
                           ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            var users = UserManager.GetAllUsersAsIQueryable();


            var cachePagesString = AppConfig.Caches.Get(CacheNames.ProfilePaginationDataCount);
            var count = -1;
            if (cachePagesString == null) {
                count = users.Select(n => n.Id).Count();
            } else {
                count = (int)cachePagesString;
            }
            // add ordered by
            users = users.OrderByDescending(n => n.Id);
            var pagesItems = (double)AppConfig.Setting.PageItems;

            var pagesExist = (int)Math.Ceiling(count / pagesItems);
            var pageInfo = new PaginationInfo {
                ItemsInPage = AppConfig.Setting.PageItems,
                PageNumber = page,
                PagesExists = pagesExist
            };
            var usersForThisPage = users.GetPageData(pageInfo, CacheNames.ProfilePaginationDataCount, false).ToList();
            const string eachUrl = "/profiles?page=@page";
            ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
                maxNumbersOfPagesShow: MaxNumbersOfPagesShow));
            return View(usersForThisPage);
        }

        /// <summary>
        /// GET: Profiles/username?page=1
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="page"></param>
        /// <returns></returns>
        //[OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        [OutputCache(CacheProfile = "Hour", VaryByParam = "*")]
        public ActionResult Profile(string username, int page = 1) {


            ApplicationUser user;
            if (UserManager.IsUserNameExistWithValidation(username, out user)) {

                var algorithms = new Algorithms();
                ViewBag.Title = username + "'s apps collection";
                ViewBag.Meta = "Mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
                ViewBag.Keywords = ViewBag.Meta;

                if (user != null) {
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
                    algorithms.GetEmbedImagesWithApp(appsForThisPage, db, (int)pageInfo.ItemsInPage,
                        GalleryCategoryIDs.SearchIcon);
                    ViewBag.Apps = appsForThisPage;
                    var eachUrl = "/profiles/" + user.UserName + "?page=@page";
                    ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
                        maxNumbersOfPagesShow: MaxNumbersOfPagesShow));
                    ViewBag.breadcrumbs = algorithms.GetBredcrumbsBasedOnCurrentUrl();
                    return View(user);
                }
            }
            ViewBag.Reason = "User not found.";
            return View("_404");
        }
    }
}