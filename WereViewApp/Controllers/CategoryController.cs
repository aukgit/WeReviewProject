#region using block

using System.Web;
using System.Web.Mvc;
using DevMvcComponent.Pagination;
using WereViewApp.BusinessLogics;
using WereViewApp.Modules.Cache;

#endregion

namespace WereViewApp.Controllers {
    public class CategoryController : Controller {
        // GET: Category
        private const int MaxNumbersOfPagesShow = 8;

        public ActionResult Index() {
            var alg = new Logics();
            var categories = alg.GetCategoryWiseAppsForCategoryPage();
            return View(categories);
        }

        public ActionResult Specific(string slug, int page = 1) {
            //categoryName= Url.de
            if (!string.IsNullOrWhiteSpace(slug)) {
                var alg = new Logics();
                var pageInfo = new PaginationInfo {
                    ItemsInPage = AppConfig.Setting.PageItems,
                    PageNumber = page
                };
                var category = alg.GetCategoryPageApps(slug,
                    pageInfo,
                    CacheNames.CategoryPageSpecificPagesCount + "-" + slug);
                if (category != null) {
                    ViewBag.Title = "Mobile apps category : " + category.CategoryName;
                    ViewBag.Meta = "Mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
                    ViewBag.Keywords = ViewBag.Meta;

                    var eachUrl = "/Apps/Category/" + category.CategoryName + "/@page";
                    ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
                        maxNumbersOfPagesShow: MaxNumbersOfPagesShow));
                    return View(category);
                }
            }

            ViewBag.Reason = "Category not found.";
            return View("_404");
        }
    }
}