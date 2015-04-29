#region using block

using System.Linq;
using System.Web.Mvc;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.WereViewAppCommon;
using DevTrends.MvcDonutCaching;

#endregion

namespace WereViewApp.Controllers {
    public class PartialsController : AdvanceController {
        #region Declarations

        private readonly Algorithms algorithms = new Algorithms();

        #endregion

        #region Constructors

        public PartialsController()
            : base(true) {
        }

        #endregion

        #region Homepage Gallery

        public ActionResult HomePageGallery() {
            var max = db.FeaturedImages.Count();
            var homePageGalleryApps = algorithms.GetHomePageGalleryImages(db, max);
            return PartialView(homePageGalleryApps);
        }

        #endregion

        #region Advertise gallery

        public ActionResult AdvertiseGallery() {
            var max = db.Galleries.Count(n => n.GalleryCategoryID == GalleryCategoryIDs.Advertise);

            var advertiseImages = algorithms.GetAdvertises(db, max);
            return PartialView(advertiseImages);
        }

        #endregion

        #region Riviews Display

        [DonutOutputCache(CacheProfile="Day", VaryByParam = "id")]
        public ActionResult ReviewsDisplay(long id) {
            var app = algorithms.GetAppFromStaticCache(id);
            if (app != null) {
                return PartialView(app);
            }
            return null;
        }

        #endregion

        #region Latest Apps

        /// <summary>
        ///     5 mins
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult LatestAppsList() {
            var latestApps = algorithms.GetLatestApps(db, 25);
            return PartialView(latestApps);
        }

        #endregion

        #region Top Apps

        [OutputCache(Duration = 86400)]
        public ActionResult TopAppsList() {
            var topApps = algorithms.GetTopRatedApps(db, 25);
            return PartialView(topApps);
        }

        #endregion

        #region Header

        [DonutOutputCache(CacheProfile = "Hour", VaryByCustom = "byuser")]
        public ActionResult NavBar() {
            //if (User.Identity.IsAuthenticated) {
            //    var userid = UserManager.GetLoggedUserId();
            //    ViewBag.Role = RoleManager.GetHighestRole(userid);
            //}
            return PartialView();
        }

        #region Menu

        public ActionResult Menu() {
            return PartialView();
        }

        #endregion

        #region Search Form
        //[DonutOutputCache(CacheProfile="TwoSec")]
        public ActionResult SearchForm() {
            return PartialView();
        }

        #endregion

        #endregion

        #region Suggested & Featured Apps

        //[OutputCache(Duration = 86400, VaryByParam = "appID")]
        public ActionResult FeaturedApps(long? appID) {
            if (appID != null) {
                var app = algorithms.GetAppFromStaticCache((long) appID);
                var featuredApps = algorithms.GetFeaturedAppsWithImages(app, db, 20);
                return PartialView(featuredApps);
            }
            return PartialView();
        }


        //[OutputCache(Duration = 86400, VaryByParam = "appID")]
        public ActionResult SuggestedApps(long? appID) {
            if (appID != null) {
                var app = algorithms.GetAppFromStaticCache((long) appID);
                var suggestedApps = algorithms.GetFinalSuggestedAppsCache(app, db);
                return PartialView(suggestedApps);
            }
            return PartialView();
        }

        #endregion
    }
}