using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.POCO;

namespace WereViewApp.WereViewAppCommon.Structs {
    public struct CommonVars {

        #region User Points
        static List<UserPointSetting> _userPointSettings;

        public static List<UserPointSetting> UserPointSettingsCache { 
            get {
                if (_userPointSettings == null) {
                    using (var db = new WereViewAppEntities()) {
                        _userPointSettings = db.UserPointSettings.ToList();
                    }
                }
                return _userPointSettings;

            }
            set {
                _userPointSettings = value;
            }
        }

        #endregion

        #region Notification Types
        static List<NotificationType> _notificationtypes;

        public static List<NotificationType> NotificationTypesCache {
            get {
                if (_notificationtypes == null) {
                    using (var db = new WereViewAppEntities()) {
                        _notificationtypes = db.NotificationTypes.ToList();
                    }
                }
                return _notificationtypes;

            }
            set {
                _notificationtypes = value;
            }
        }

        #endregion

        #region Output Cache URLS
        public const string OUTPUTCAHE_SUGGESTED_APPS = @"/Partials/SuggestedApps";
        public const string OUTPUTCAHE_FEATUREDAPPS_APPS = @"/Partials/FeaturedApps";
        public const string OUTPUTCAHE_REVIEWSDISPLAY_APPS = @"/Partials/ReviewsDisplay/";
        public const string OUTPUTCAHE_ADVERTISEGALLERY_APPS = @"/Partials/AdvertiseGallery";
        public const string OUTPUTCAHE_LATESTAPPSLIST_APPS = @"/Partials/LatestAppsList";
        public const string OUTPUTCAHE_TOPAPPSLIST_APPS = @"/Partials/TopAppsList";
        

        #endregion

        #region Caching Data In file Locations

        public const string APP_VIRTUAL_FIELDS_SAVING_ADDITIONALPATH = @"Database\";
        public const string APP_SUGGESTED_ADDITIONALPATH = @"Suggested\";
        public const string APP_SEARCH_RESULTS_ADDITIONALPATH = @"SearchResult\";

        #endregion

        #region Apps Already Found
        public static List<App> AppsFoundForSingleDisplay { get; set; }

        #endregion

        #region Reg expressions
        public const string FRIENDLY_URL_REGEX = @"[^A-Za-z0-9_\.~]+";
        #endregion

        #region regular constants
        public static string[] SearchingEscapeSequence = new string[] 
                      { "is", "were", "what", 
                        "i", "am", "was", 
                        "have", "in", "ain't", 
                        "hello", "find", "lol", 
                        "has", "for", "his", 
                        "her", "vs", "v", 
                        "v.","lmao","rofl",
                        "new","old", "vs"};

        #endregion

        #region Data Saving on File

        public const string APP_SAVING_EXTENSION = @".mdb";

        #endregion

        #region Location of Images in Gallery Constants

        public const string ADDITIONAL_ROOT_ADVERTISE_LOCATION = "Advertise/";

        /// <summary>
        /// "Gallery/"
        /// </summary>
        public const string ADDITIONAL_ROOT_GALLERY_LOCATION = "Gallery/";
        /// <summary>
        /// "GalleryThumbs/"
        /// </summary>
        public const string ADDITIONAL_ROOT_GALLERY_ICON_LOCATION = "GalleryThumbs/";
        /// <summary>
        /// "SearchThumbs/"
        /// </summary>
        public const string ADDITIONAL_ROOT_SEARCH_ICON_LOCATION = "SearchThumbs/";
        /// <summary>
        /// "HomePageThumbs/"
        /// </summary>
        public const string ADDITIONAL_ROOT_HOME_ICON_LOCATION = "HomePageThumbs/";
        /// <summary>
        /// "HomePageFeatured/"
        /// </summary>
        public const string ADDITIONAL_ROOT_HOME_LOCATION = "HomePageFeatured/";
        /// <summary>
        /// "SuggestionThumbs/"
        /// </summary>
        public const string ADDITIONAL_ROOT_SUGGESTED_ICON_LOCATION = "SuggestionThumbs/";

        #endregion

        #region Apps Suggestions Number
        public const int SUGGEST_HIGHEST_TAKE = 10;
        public const int SUGGEST_HIGHEST_DISPLAY_NUMBER_SUGGESTIONS = 12;
        public const int SUGGEST_HIGHEST_FROM_SAME_USER = 3;
        public const int SUGGEST_HIGHEST_SAME_APP_NAME = 3;
        public const int SUGGEST_HIGHEST_AND_SIMILAR_QUERY = 10;
        public const int SUGGEST_HIGHEST_OR_SIMILAR_QUERY = 10;

        public const int SEARCH_RESULTS_MAX_RESULT_RETURN = 80;


        #endregion
        #region Truncate Len
        public const int APP_HOME_PAGE_DESCRIPTION_TRUNC_LEN = 45;
        
        #endregion

        #region Cache in file max expire time

        public const int APP_SEARCH_RESULTS_EXPIRE_IN_HOURS = 36;
        public const int APP_SUGGESTED_RESULTS_EXPIRE_IN_HOURS = 36;

        #endregion
    }
}