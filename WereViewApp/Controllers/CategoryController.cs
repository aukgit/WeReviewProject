using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index() {
            var alg = new Algorithms();
            var categories = alg.GetCategoryWiseAppsForCategoryPage();
            return View(categories);
        }
    }
}