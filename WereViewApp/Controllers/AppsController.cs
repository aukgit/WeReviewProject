#region using block

using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Modules.DevUser;
using WereViewApp.WereViewAppCommon;


#endregion

namespace WereViewApp.Controllers {
    public class AppsController : AdvanceController {
        #region Declarations

        private readonly Algorithms _algorithms = new Algorithms();

        #endregion

        #region Constructors

        public AppsController()
            : base(true) {
        }

        #endregion

        [OutputCache(CacheProfile = "Day")]
        public ActionResult Index(int page = 1) {
            HtmlString paginationHtml;
            var archivedApps = _algorithms.GetLatestApps(db, true, page, out paginationHtml);
            ViewBag.Title = "Apps";
            ViewBag.Meta = "Latest mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            ViewBag.Apps = archivedApps;
            ViewBag.paginationHtml = paginationHtml;
            return View("Index");
        }

        [OutputCache(CacheProfile = "Day")]
        public ActionResult Latest() {
            var latest = _algorithms.GetLatestApps(db, 60);
            ViewBag.Title = "Latest mobile apps";
            ViewBag.Meta = "Latest mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            ViewBag.Apps = latest;
            return View("Index");
        }

        [OutputCache(CacheProfile = "Day")]
        public ActionResult Top() {
            var top = _algorithms.GetTopRatedApps(db, 100);
            ViewBag.Title = "Top 100 mobile apps";
            ViewBag.Meta = "Latest mobile apps, apps review, apple apps, android apps, " + ViewBag.Title;
            ViewBag.Keywords = ViewBag.Meta;
            ViewBag.Apps = top;
            return View("Index");
        }
        [Authorize]
        public ActionResult Reviewed() {
            ViewBag.Title = "App Reviewed By You";
            var userid = UserManager.GetLoggedUserId();
            var reviews = db.Reviews.Include(r => r.App).Include(r => r.User).Where(n => n.UserID == userid);
            return View(reviews.ToList());
        }

    }
}