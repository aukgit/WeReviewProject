#region using block

using System.Web.Mvc;
using WereViewApp.Models.ViewModels;
using WereViewApp.WereViewAppCommon;
using WereViewApp.WereViewAppCommon.Structs;

#endregion

namespace WereViewApp.Controllers {
    public class SearchController : AdvanceController {
        private readonly Algorithms _algorithms = new Algorithms();

        protected SearchController() : base(true) {
            
        }
        #region Search
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(CacheProfile = "Long", VaryByParam = "url")]
        public ActionResult Index(SearchViewModel search, string url) {
            ViewBag.isPostBack = true;
            if (!string.IsNullOrWhiteSpace(url)) {
                var urlGet = _algorithms.GenerateURLValid(url);
                var displayList = urlGet.Split('-');
                var displayStr = string.Join(" ", displayList);
                var results = _algorithms.GetSearchResults(url, null, null, null,
                    CommonVars.SEARCH_RESULTS_MAX_RESULT_RETURN, db);
                search.SearchQuery = displayStr;
                ViewBag.Results = results;
                return View(search);
            }
            ViewBag.Results = null;
            return View();
        }

        #endregion
    }
}