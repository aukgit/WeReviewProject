#region using block

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using WereViewApp.Models.Context;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.ExtenededWithCustomMethods;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Modules.Session;
using WereViewApp.WereViewAppCommon;

#endregion

namespace WereViewApp.Controllers {
    public class PartialsController : AdvanceController {

        #region Drop downs
        [OutputCache(CacheProfile = "Day")]
        public ActionResult GetFeedbackCategoryID() {
            if (SessionNames.IsValidationExceed("GetFeedbackCategoryID", 100)) {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            using (var db = new ApplicationDbContext()) {
                var categories = db.FeedbackCategories
                    .Select(n => new { display = n.Category, id = n.FeedbackCategoryID })
                    .ToList();
                return Json(categories, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        //#region Drop down : Country, timezone, language
        ////[OutputCache(CacheProfile = "YearNoParam")]
        ////public JsonResult GetCountryId() {
        ////    //var countries = CachedQueriedData.GetCountries();

        ////    var countries = CachedQueriedData.GetCountries().Select(n => new {
        ////        display = n.DisplayCountryName,
        ////        id = n.CountryID,
        ////        countryCode = n.Alpha2Code.ToLower()
        ////    }).ToList();
        ////    //return HtmlHelpers.DropDownCountry(countries);
        ////    return Json(countries, JsonRequestBehavior.AllowGet);
        ////}

        ////public string GetCountryId(string id) {
        ////    //var countries = CachedQueriedData.GetCountries();
        ////    //var countryId = IpConfigRelations.GetCountryId(id);
        ////    Country country = null;

        ////    var value = IpConfigRelations.IpToValue(id);
        ////    using (var db = new ApplicationDbContext()) {
        ////        //SELECT * FROM [ip-to-country] WHERE (([BeginingIP] <= ?) AND ([EndingIP] >= ?))
        ////        var countryIp = db.CountryDetectByIPs.FirstOrDefault(n => n.BeginingIP <= value && n.EndingIP >= value);
        ////        if (countryIp != null) {
        ////            country = CachedQueriedData.GetCountries().FirstOrDefault(n =>
        ////               n.CountryID == countryIp.CountryID
        ////           );
        ////            if (country != null) {
        ////                return country.DisplayCountryName + " : val : " + value + ", ip :" + id;
        ////            }
        ////        }
        ////    }
        ////    return "-1 : " + id + " : " + value;

        ////    //return HtmlHelpers.DropDownCountry(countries);
        ////}

        //[OutputCache(CacheProfile = "Year", VaryByParam = "id")]
        //public ActionResult GetTimeZone(int id) {
        //    if (SessionNames.IsValidationExceed("GetTimeZone", 100)) {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //    var getZones = CachedQueriedData.GetTimezones(id);
        //    if (getZones != null) {
        //        var represent = getZones.Select(n => new { display = n.Display, id = n.UserTimeZoneID });
        //        return Json(represent.ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(null, JsonRequestBehavior.AllowGet);
        //}

        ////[OutputCache(CacheProfile = "Day", VaryByParam = "id")]
        //public ActionResult GetLanguage(int id) {
        //    if (SessionNames.IsValidationExceed("GetLanguage", 100)) {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //    var languges = CachedQueriedData.GetLanguages(id);
        //    if (languges != null) {
        //        var represent =
        //            languges.Select(n => new { display = n.Language + " - " + n.NativeName, id = n.CountryLanguageID });
        //        return Json(represent.ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(null, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        [OutputCache(CacheProfile = "TenMins", VaryByParam = "*")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTags(string id) {
            if (SessionNames.IsValidationExceed("GetTags", 500) || string.IsNullOrWhiteSpace(id)) {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
      
            using (var db = new WereViewAppEntities()) {
                var tags = db.Tags.Where(n => n.TagDisplay.StartsWith(id))
                                  .Select(n => n.TagDisplay).Take(10).ToArray();
               
                var list = new List<string>(25);
                foreach (var tag in tags) {
                    list.Add(tag);
                }
                if (id.Length > 3) {
                    var tags2 = db.Tags.Where(n => n.TagDisplay.Contains(id) && tags.All(found => found != n.TagDisplay))
                                      .Select(n => n.TagDisplay).Take(10).ToArray();
                    foreach (var tag in tags2) {
                        list.Add(tag);
                    }
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            };
        }


        [OutputCache(NoStore = true, Duration = 0)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAppUrl(App app) {
            if (SessionNames.IsValidationExceed("GetAppUrl", 500) || app == null) {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            using (var db = new WereViewAppEntities()) {
                var algorithms = new Algorithms();

                app.Url = algorithms.GenerateHyphenUrlStringValid(app, db);

                var sender = new {
                    url = app.GetAbsoluteUrl()
                };
                return Json(sender, JsonRequestBehavior.AllowGet);
            };
        }

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

        [DonutOutputCache(CacheProfile = "Day", VaryByParam = "id")]
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

        #region Header : Navigation

        [DonutOutputCache(Duration = 35, VaryByCustom = "byuser")]
        public ActionResult NavBar() {
            return PartialView();
        }

        #endregion

        #region Suggested + Featured Apps + Developer's App

        //[OutputCache(Duration = 86400, VaryByParam = "appID")]
        public ActionResult FeaturedApps(long? appID) {
            if (appID != null) {
                var app = algorithms.GetAppFromStaticCache((long)appID);
                var featuredApps = algorithms.GetFeaturedAppsWithImages(app, db, 20);
                return PartialView(featuredApps);
            }
            return PartialView();
        }


        [OutputCache(Duration = 86400, VaryByParam = "appID")]
        public ActionResult SuggestedApps(long? appID) {
            if (appID != null) {
                var app = algorithms.GetAppFromStaticCache((long)appID);
                var suggestedApps = algorithms.GetFinalSuggestedAppsCache(app, db);
                return PartialView(suggestedApps);
            }
            return PartialView();
        }

        [OutputCache(Duration = 86400, VaryByParam = "appID")]
        public ActionResult DevelopersApps(long? appID) {
            if (appID != null) {
                var app = algorithms.GetAppFromStaticCache((long)appID);
                var suggestedApps = algorithms.GetDevelopersAppsByApp(app, db); // logic needs to be written

                return PartialView(suggestedApps);
            }
            return PartialView();
        }


        #endregion
    }
}