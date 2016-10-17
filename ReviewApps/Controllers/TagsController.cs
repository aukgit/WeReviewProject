using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevMvcComponent.Pagination;
using ReviewApps.BusinessLogics;
using ReviewApps.Constants;
using ReviewApps.Models.EntityModel.Structs;
using ReviewApps.Modules.Cache;
using ReviewApps.Modules.Extensions;
namespace ReviewApps.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public class TagsController : AdvanceController {
        // GET: Tags
        private const int MaxNumbersOfPagesShow = 8;

        #region Declarations

        private readonly Logics _logics = new Logics();

        #endregion

        #region Constructors

        public TagsController()
            : base(true) {
            ControllerUrl = "/" + this.GetControllerName();
        }

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
            string eachUrl = ControllerUrl + "?page=@page";
            ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
                maxNumbersOfPagesShow: MaxNumbersOfPagesShow));
            return View(tagsforThisPage);
        }

        //               ViewBag.Title;
        //ViewBag.Meta = "Tags , Mobile apps, apps review, apple apps, android apps,reviews, app review site, " +
        //ViewBag.Title = "Mobile Applications Tags";

        public ActionResult GetTagDetail(string id, int page = 1) {
            if (string.IsNullOrWhiteSpace(id)) {
                return HttpNotFound();
            }
            var tagname = Logics.GetAllUpperCaseTitle(id);
            ViewBag.Keywords = ViewBag.Meta;
            var cacheName = "Tags.GetTagDetail." + id;
            var apps = _logics.GetViewableApps(db)
                                    .Where(n => n.TagAppRelations.Any(tagRel => tagRel.Tag.TagDisplay == tagname))
                                    .Include(n => n.User)
                                    .OrderByDescending(n => n.AppID);
            int maxItems = (int)AppConfig.Setting.PageItems;

            var pageInfo = new PaginationInfo {
                ItemsInPage = maxItems,
                PageNumber = page,
            };
            var appsForThisPage =
                apps.GetPageData(pageInfo, CacheNames.ProfilePaginationDataForSpecificProfile, true)
                    .ToList();
            _logics.GetEmbedImagesWithApp(appsForThisPage, db, maxItems, GalleryCategoryIDs.SearchIcon);
            ViewBag.Apps = appsForThisPage;
            var eachUrl = ControllerUrl + "/" + id + "?page=@page";
            ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "", maxNumbersOfPagesShow: MaxNumbersOfPagesShow));
            ViewBag.tagName = tagname;
            ViewBag.breadcrumbs = _logics.GetBredcrumbsBasedOnCurrentUrl();
            return View();
        }
    }
}