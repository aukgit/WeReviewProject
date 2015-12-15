using System.Web.Mvc;
using WereViewApp.WereViewAppCommon;
using System.Linq;
using System.Data.Entity;
using System.Web;
using DevMvcComponent.Pagination;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Modules.Cache;

namespace WereViewApp.Controllers {
    public class TagsController : AdvanceController {
        #region Declarations

        private readonly Algorithms _algorithms = new Algorithms();

        #endregion

        #region Constructors

        public TagsController()
            : base(true) {
        }

        #endregion
        // GET: Tags
        private const int MaxNumbersOfPagesShow = 8;

        //[OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        //[OutputCache(CacheProfile = "Hour", VaryByParam = "*")]
        public ActionResult Index(string id, int page = 1) {
            if (!string.IsNullOrWhiteSpace(id)) {
                return GetTagDetail(id, page);
            }
            ViewBag.Title = "Mobile Applications Tags";
            ViewBag.Meta = "Tags , Mobile apps, apps review, apple apps, android apps,reviews, app review site, " +
                           ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            var cacheName = "Tags.names";
            var tags = db.Tags.OrderByDescending(n => n.TagID);

            // add ordered by
            var pageInfo = new PaginationInfo {
                ItemsInPage = AppConfig.Setting.PageItems,
            };
            var tagsforThisPage = tags.GetPageData(pageInfo, cacheName).ToList();
            const string eachUrl = "/tags?page=@page";
            ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
                maxNumbersOfPagesShow: MaxNumbersOfPagesShow));
            return View(tagsforThisPage);
        }

        public ActionResult GetTagDetail(string id, int page = 1) {
            //ViewBag.Title = "Mobile Applications Tags";
            //ViewBag.Meta = "Tags , Mobile apps, apps review, apple apps, android apps,reviews, app review site, " +
            //               ViewBag.Title;
            //ViewBag.Keywords = ViewBag.Meta;
            //var cacheName = "Tags.GetTagDetail." + id;
            //var tags = db.Tags.OrderByDescending(n => n.TagID);
            //var apps = _algorithms.GetViewableApps(db)
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
            //_algorithms.GetEmbedImagesWithApp(appsForThisPage, db, (int)pageInfo.ItemsInPage,
            //    GalleryCategoryIDs.SearchIcon);
            //ViewBag.Apps = appsForThisPage;
            //var eachUrl = "/profiles/" + user.UserName + "?page=@page";
            //ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
            //    maxNumbersOfPagesShow: MaxNumbersOfPagesShow));
            //return System.Web.UI.WebControls.View(user);
        }
    }
}