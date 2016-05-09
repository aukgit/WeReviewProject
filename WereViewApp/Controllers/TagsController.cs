using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevMvcComponent.Pagination;
using WeReviewApp.BusinessLogics;

namespace WeReviewApp.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public class TagsController : AdvanceController {
        // GET: Tags
        private const int MaxNumbersOfPagesShow = 8;

        #region Declarations

        private readonly Logics _logics = new Logics();

        #endregion

        #region Constructors

        public TagsController()
            : base(true) {}

        #endregion

        //[OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        //[OutputCache(CacheProfile = "Hour", VaryByParam = "*")]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Index(int page = 1) {
            ViewBag.Title = "Mobile Applications Tags";
            ViewBag.Meta = "Tags , Mobile apps, apps review, apple apps, android apps,reviews, app review site, " +
                           ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            var cacheName = "Tags.names";
            var tags = db.Tags.OrderByDescending(n => n.TagID);

            // add ordered by
            var pageInfo = new PaginationInfo {
                ItemsInPage = 100,
                PageNumber = page
            };
            var tagsforThisPage = tags.GetPageData(pageInfo, cacheName).ToList();
            const string eachUrl = "/tags?page=@page";
            ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
                maxNumbersOfPagesShow: MaxNumbersOfPagesShow));
            return View(tagsforThisPage);
        }

        //               ViewBag.Title;
        //ViewBag.Meta = "Tags , Mobile apps, apps review, apple apps, android apps,reviews, app review site, " +
        //ViewBag.Title = "Mobile Applications Tags";

        //public ActionResult GetTagDetail(string id, int page = 1) {
        //ViewBag.Keywords = ViewBag.Meta;
        //var cacheName = "Tags.GetTagDetail." + id;
        //var tags = db.Tags.OrderByDescending(n => n.TagID);
        //var apps = _logics.GetViewableApps(db)
        //                        .Where(n => n.TagAppRelations. == user.UserID)
        //                        .Include(n => n.User)
        //                        .OrderByDescending(n => n.AppID);

        //var pageInfo = new PaginationInfo {
        //    ItemsInPage = AppConfig.Setting.PageItems,
        //    PageNumber = page,
        //    PagesExists = -1
        //};
        //var appsForThisPage =
        //    apps.GetPageData(pageInfo, CacheNames.ProfilePaginationDataForSpecificProfile, true)
        //        .ToList();
        //_logics.GetEmbedImagesWithApp(appsForThisPage, db, (int)pageInfo.ItemsInPage,
        //    GalleryCategoryIDs.SearchIcon);
        //ViewBag.Apps = appsForThisPage;
        //var eachUrl = "/profiles/" + user.UserName + "?page=@page";
        //ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
        //    maxNumbersOfPagesShow: MaxNumbersOfPagesShow));
        //return View(user);
        //}
    }
}