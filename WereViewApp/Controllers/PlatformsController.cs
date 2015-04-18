#region using block

using System.Web.Mvc;
using DevMVCComponent.Database;
using WereViewApp.Modules.Cache;
using WereViewApp.WereViewAppCommon;

#endregion

namespace WereViewApp.Controllers {
    public class PlatformsController : Controller {
        // GET: Category
        private readonly int MaxNumbersOfPagesShow = 8;

        public ActionResult Index() {
            var alg = new Algorithms();
            var platforms = alg.GetPlatformWiseAppsForPlatformPage();
            return View(platforms);
        }

        public ActionResult Specific(string platformName, int page = 1) {
            if (!string.IsNullOrWhiteSpace(platformName)) {
                var alg = new Algorithms();
                var pageInfo = new PaginationInfo {
                    ItemsInPage = AppConfig.Setting.PageItems,
                    PageNumber = page
                };
                var platform = alg.GetPlatformPageApps(platformName,
                    pageInfo,
                    CacheNames.PlatformPageSpecificPagesCount + "-" + platformName);
                if (platform != null) {
                    ViewBag.Title = "Apps Review platform : " + platform.PlatformName;
                    ViewBag.Meta = "Mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
                    ViewBag.Keywords = ViewBag.Meta;

                    var eachUrl = "/Platform/" + platform.PlatformName + "/@page";
                    ViewBag.paginationHtml = Pagination.GetList(pageInfo, eachUrl, "",
                        maxNumbersOfPagesShow: MaxNumbersOfPagesShow);
                    return View(platform);
                }
            }

            ViewBag.Reason = "Category not found.";
            return View("_404");
        }
    }
}