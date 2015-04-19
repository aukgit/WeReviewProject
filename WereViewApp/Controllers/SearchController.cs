using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Models.ViewModels;
using WereViewApp.Modules.Session;
using WereViewApp.WereViewAppCommon;
using WereViewApp.WereViewAppCommon.Structs;

namespace WereViewApp.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(CacheProfile = "Long", VaryByParam = "SearchQuery")]
        public ActionResult Index(string SearchQuery) {
            var max = 60;

            SearchViewModel search = new SearchViewModel();
            var algorithms = new Algorithms();
            //ViewBag.isPostBack = true;
            if (!string.IsNullOrWhiteSpace(SearchQuery)) {
                if (!AppVar.Setting.IsInTestingEnvironment) {
                    if (SessionNames.IsValidationExceed("SearchingFormCount", max)) {
                        ViewBag.Reason = "You have exceed your search cases. Perhaps you should try tomorrow.";
                        return View("_429");
                    }
                }
                search.SearchQuery = SearchQuery;
                var urlGet = algorithms.GenerateURLValid(SearchQuery);
                var displayList = urlGet.Split('-');
                var displayStr = string.Join(" ", displayList);
                var results = algorithms.GetSearchResults(SearchQuery, null, null, null,
                                         CommonVars.SearchResultsMaxResultReturn);
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