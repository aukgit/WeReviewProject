using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevMVCComponent.Database;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Modules.Cache;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Controllers {
    public class CategoryController : Controller {
        // GET: Category
        readonly int MaxNumbersOfPagesShow = 8;

        public ActionResult Index() {
            var alg = new Algorithms();
            var categories = alg.GetCategoryWiseAppsForCategoryPage();
            return View(categories);
        }

        public ActionResult Specific(string categoryName, int page = 1) {
            if (!string.IsNullOrWhiteSpace(categoryName)) {
                var alg = new Algorithms();
                var pageInfo = new PaginationInfo {
                    ItemsInPage = AppConfig.Setting.PageItems,
                    PageNumber = page
                };
                var category = alg.GetCategoryPageApps(categoryName, 
                                pageInfo, 
                                CacheNames.CategoryPageSpecificPagesCount + "-" + categoryName);
                if (category != null) {
                    ViewBag.Title = "Mobile apps category : " + category.CategoryName;
                    ViewBag.Meta = "Mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
                    ViewBag.Keywords = ViewBag.Meta;

                    var eachUrl = "/Apps/Category/" + category.CategoryName + "/@page";
                    ViewBag.paginationHtml = Pagination.GetList(pageInfo, eachUrl, "",
                        maxNumbersOfPagesShow: MaxNumbersOfPagesShow);
                    return View(category);
                }
            }

            ViewBag.Reason = "Category not found.";
            return View("_NotExist");
        }

    }
}