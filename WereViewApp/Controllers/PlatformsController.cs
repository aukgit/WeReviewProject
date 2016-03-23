#region using block

using System.Web.Mvc;
using DevMvcComponent.Pagination;
using WereViewApp.BusinessLogics;
using WereViewApp.Modules.Cache;

#endregion

namespace WereViewApp.Controllers {
    public class PlatformsController : Controller {
        private readonly int MaxNumbersOfPagesShow = 8;
        // GET: 

        public ActionResult Index() {
            var alg = new Logics();
            var platforms = alg.GetPlatformWiseAppsForPlatformPage();
            return View(platforms);
        }

        public ActionResult Specific(string platformName, int page = 1) {
            if (!string.IsNullOrWhiteSpace(platformName)) {
                var alg = new Logics();
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