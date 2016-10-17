using System.Web.Mvc;
using System.Web.UI;
using ReviewApps.BusinessLogics;
using ReviewApps.Common;
using ReviewApps.Models.ViewModels;
using ReviewApps.Modules.Session;

namespace ReviewApps.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public class SearchController : Controller {
        // GET: Search
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(CacheProfile = "Long", VaryByParam = "SearchQuery")]
        public ActionResult Index(string SearchQuery) {
            var max = 60;

            var search = new SearchViewModel();
            var algorithms = new Logics();
            //ViewBag.isPostBack = true;
            if (!string.IsNullOrWhiteSpace(SearchQuery)) {
                if (!AppVar.Setting.IsInTestingEnvironment) {
                    if (SessionNames.IsValidationExceed("SearchingFormCount", max)) {
                        var errorRoute = new ErrorsController();
                        return errorRoute.Error(429, null,
                            "You have exceed your search cases. Perhaps you should try tomorrow.");
                    }
                }
                search.SearchQuery = SearchQuery;
                var urlGet = algorithms.GenerateHyphenUrlString(SearchQuery);
                var displayList = urlGet.Split('-');
                var displayStr = string.Join(" ", displayList);
                var results = algorithms.GetSearchResults(SearchQuery, null, null, null,
                    Variables.SearchResultsMaxResultReturn);
                search.DisplayStringToUser = displayStr;
                search.FoundApps = results;
                return View(search);
            }
            search.DisplayStringToUser = "";
            search.FoundApps = null;
            return View(search);
        }
    }
}