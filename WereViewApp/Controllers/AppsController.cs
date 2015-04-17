using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.WereViewAppCommon;
using WereViewApp.WereViewAppCommon.Structs;
using WereViewApp.Modules.DevUser;
using WereViewApp.Models.ViewModels;
using WereViewApp.Filter;

namespace WereViewApp.Controllers {
    
    public class AppsController : AdvanceController {

        #region Declarations

        readonly Algorithms _algorithms = new Algorithms();
        #endregion

        #region Constructors
        public AppsController()
            : base(true) {

        }
        #endregion


        #region Search

        public ActionResult Search() {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(CacheProfile = "Long", VaryByParam = "Url")]
        public ActionResult Search(SearchViewModel search, string url) {
            ViewBag.isPostBack = true;
            if (!string.IsNullOrWhiteSpace(url)) {
                var urlGet = _algorithms.GenerateURLValid(url);
                var displayList = urlGet.Split('-');
                var displayStr = string.Join(" ", displayList);
                var results = _algorithms.GetSearchResults(url, null, null, null, CommonVars.SEARCH_RESULTS_MAX_RESULT_RETURN, db);
                search.DisplaySearchText = displayStr;
                ViewBag.Results = results;
                return View(search);
            } else {
                ViewBag.Results = null;
                return View();
            }
        }
        #endregion

        [OutputCache(CacheProfile = "Day")]
        public ActionResult Latest() {
            ViewBag.Title = "Latest Apps";

            var latest = _algorithms.GetLatestApps(db, 60);
            return View("ListOfApps", latest);
        }
        [OutputCache(CacheProfile = "Day")]
        public ActionResult TopRated() {
            ViewBag.Title = "Top Rated Apps";
            var latest = _algorithms.GetTopRatedApps(db, 60);
            return View("ListOfApps", latest);
        }

        [OutputCache(CacheProfile = "Day", VaryByParam = "id")]
        public ActionResult Category(string id) {
            var cat = WereViewStatics.AppCategoriesCache.FirstOrDefault(n => n.CategoryName == id);
            if (cat != null) {
                ViewBag.Title = "Category : " + id;
                const int max = 60;
                var categoryApps = db.Apps.Where(n => n.CategoryID == cat.CategoryID)
                                        .OrderByDescending(n => n.TotalViewed)
                                        .ThenByDescending(n => n.AppID)
                                        .Include(n => n.Platform)
                                        .Take(max)
                                        .ToList();

                if (categoryApps != null) {
                    _algorithms.GetEmbedImagesWithApp(categoryApps, db, max, GalleryCategoryIDs.HomePageIcon);
                }
                return View("ListOfApps", categoryApps);
            }
            return HttpNotFound();
        }

        [Authorize]
        public ActionResult Reviewed() {
            ViewBag.Title = "App Reviewed By You";
            var userid = UserManager.GetLoggedUserId();
            var reviews = db.Reviews.Include(r => r.App).Include(r => r.User).Where(n => n.UserID == userid);
            return View(reviews.ToList());

        }
        [OutputCache(CacheProfile = "Day")]
        public ActionResult IOs() {
            ViewBag.Title = "Apple/iOS Apps";

            return PlatformResult(PlatformIDs.iOS);

        }
        [OutputCache(CacheProfile = "Day")]
        public ActionResult Windows() {
            ViewBag.Title = "Windows Apps";
            return PlatformResult(PlatformIDs.Windows);

        }
        [OutputCache(CacheProfile = "Day")]

        public ActionResult Android() {
            ViewBag.Title = "Android Apps";

            return PlatformResult(PlatformIDs.Android);
        }

        public ActionResult PlatformResult(byte platformId) {
            int max = 60;
            var platformApps = db.Apps.Where(n => n.PlatformID == platformId)
                                    .OrderByDescending(n => n.TotalViewed)
                                    .OrderByDescending(n => n.AppID)
                                    .Include(n => n.Platform)
                                    .Take(max)
                                    .ToList();

            if (platformApps != null) {
                _algorithms.GetEmbedImagesWithApp(platformApps, db, max, GalleryCategoryIDs.HomePageIcon);
            }
            return View("ListOfApps", platformApps);

        }

    }
}
