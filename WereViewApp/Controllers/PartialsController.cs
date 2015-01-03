using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.AlgorithmsWereViewApp;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Role;
using WereViewApp.WereViewAppCommon.Structs;

namespace WereViewApp.Controllers {

    public class PartialsController : AdvanceController {

        #region Declarations
        Algorithms algorithms = new Algorithms();
        #endregion

        #region Constructors
        public PartialsController()
            : base(true) {

        }
        #endregion

        #region Header
        //[OutputCache(CacheProfile = "Hour")]
        public ActionResult NavBar() {
            if (User.Identity.IsAuthenticated) {
                var userid = UserManager.GetLoggedUserId();
                ViewBag.Role = RoleManager.GetHighestRole(userid);
            }
            return PartialView();
        }

        #region Menu


        public ActionResult Menu() {
            return PartialView();
        }
        #endregion
        #region Search Form
        public ActionResult SearchForm() {
            return PartialView();
        }
        #endregion

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

        #region Show Reivew

        [OutputCache(Duration = 30, VaryByParam = "id")]
        public ActionResult ReviewsDisplay(long id) {
            var app = algorithms.GetAppFromStaticCache(id);
            if (app != null) {
                return PartialView(app);
            }
            return null;
        }


        #endregion
        #region Suggested & Featured Apps

        //[OutputCache(Duration = 86400, VaryByParam = "appID")]
        public ActionResult FeaturedApps(long? appID) {
            if (appID != null) {
                var app = algorithms.GetAppFromStaticCache((long)appID);
                var featuredApps = algorithms.GetFeaturedOnlyImages(app, db, 4);
                return PartialView(featuredApps);
            }
            return PartialView();
        }


        //[OutputCache(Duration = 86400, VaryByParam = "appID")]
        public ActionResult SuggestedApps(long? appID) {
            if (appID != null) {
                var app = algorithms.GetAppFromStaticCache((long)appID);
                var suggestedApps = algorithms.GetFinalSuggestedAppsCache(app, db);
                return PartialView(suggestedApps);
            }
            return PartialView();

        }
        #endregion

        #region Latest Apps
        /// <summary>
        /// 5 mins
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300)]
        public ActionResult LatestAppsList() {
            var advertiseImages = algorithms.GetLatestApps(db, 5);
            return PartialView(advertiseImages);
        }
        #endregion

        #region Top Apps
        [OutputCache(Duration = 86400)]

        public ActionResult TopAppsList() {
            var advertiseImages = algorithms.GetTopRatedApps(db, 10);
            return PartialView(advertiseImages);
        }
        #endregion
    }
}