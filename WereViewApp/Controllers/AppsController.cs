#region using block

using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeReviewApp.BusinessLogics;
using WeReviewApp.Modules.DevUser;

#endregion

namespace WeReviewApp.Controllers {
    public class AppsController : AdvanceController {
        #region Declarations

        private readonly Logics _algorithms = new Logics();

        #endregion

        #region Constructors

        public AppsController()
            : base(true) {}

        #endregion

        [OutputCache(CacheProfile = "Day")]
        public ActionResult Index(int page = 1) {
            HtmlString paginationHtml;
            var archivedApps = _algorithms.GetLatestApps(db, true, page, out paginationHtml, false);
            ViewBag.Title = "Apps";
            ViewBag.Meta = "Latest mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            ViewBag.Apps = archivedApps;
            ViewBag.paginationHtml = paginationHtml;
            ViewBag.breadcrumbs = _algorithms.GetBredcrumbsBasedOnCurrentUrl();
            return View("Index");
        }

        [OutputCache(CacheProfile = "Day")]
        public ActionResult Latest() {
            var latest = _algorithms.GetLatestApps(db, 60, false);
            ViewBag.Title = "Latest mobile apps";
            ViewBag.Meta = "Latest mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            ViewBag.Apps = latest;
            ViewBag.breadcrumbs = _algorithms.GetBredcrumbsBasedOnCurrentUrl();
            return View("Index");
        }

        [OutputCache(CacheProfile = "Day")]
        public ActionResult Top() {
            var top = _algorithms.GetTopRatedApps(db, 100, false);
            ViewBag.Title = "Top 100 mobile apps";
            ViewBag.Meta = "Latest mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            ViewBag.Apps = top;
            // the reason displaying HomeIcon instead of SearchIcon is that GetTopRatedApps() is common for home page and here
            // and it attaches HomeIcon instead of SearchIcon
            ViewBag.breadcrumbs = _algorithms.GetBredcrumbsBasedOnCurrentUrl();
            return View("Index");
        }

        [Authorize]
        public ActionResult Reviewed() {
            ViewBag.Title = "App Reviewed By You";
            var userid = UserManager.GetLoggedUserId();
            var reviews = db.Reviews.Include(r => r.App).Include(r => r.User).Where(n => n.UserID == userid);
            return View(reviews.ToList());
        }

        /// <summary>
        ///     Get apps filtered by :
        ///     site.com/Apps/Apple-8/Games or
        ///     site.com/Apps/Apple-8 or
        ///     site.com/Apps/Apple/Games or
        ///     site.com/Apps/Apple
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="platformVersion"></param>
        /// <param name="category"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        //[OutputCache(CacheProfile = "Day", VaryByParam = "platform;platformVersion;category;page")]
        public ActionResult GetByPlatformAndCategory(string platform, double? platformVersion, string category,
            int page = 1) {
            var apps = _algorithms.GetAppsFilteredByPlatformAndCategory(platform, platformVersion, category, page,
                ViewBag, db);
            if (apps != null) {
                ViewBag.Title = "Apps : " + _algorithms.GetCurrentUrlWithoutHostNameWithoutSlash();
                ViewBag.Meta = ViewBag.Title;
                ViewBag.Keywords = ViewBag.Meta;
                ViewBag.Apps = apps;
                ViewBag.breadcrumbs = _algorithms.GetBredcrumbsBasedOnCurrentUrl();
                return View("Index");
            }
            return View("_AppNotFound");
        }
    }
}