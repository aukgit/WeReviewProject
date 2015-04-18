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
                    var eachUrl = "/Apps/Mobile/Platforms/" + platform.PlatformName + "/@page";
                    ViewBag.paginationHtml = Pagination.GetList(pageInfo, eachUrl, "",
                        maxNumbersOfPagesShow: MaxNumbersOfPagesShow);
                    return View(platform);
                }
            }

            ViewBag.Reason = "Platform not found. Please try /Android or /Apple";
            return View("_404");
        }
    }
}