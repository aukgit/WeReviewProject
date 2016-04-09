using System.Collections.Generic;
using System.Linq;
using WeReviewApp.Models.EntityModel;

namespace WeReviewApp.WereViewAppCommon.Structs {
    public static class CommonVars {

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
        public const string OutputcaheSuggestedApps = @"/Partials/SuggestedApps";
        public const string OutputcaheFeaturedappsApps = @"/Partials/FeaturedApps";
        public const string OutputcaheReviewsdisplayApps = @"/Partials/ReviewsDisplay/";
        public const string OutputcaheAdvertisegalleryApps = @"/Partials/AdvertiseGallery";
        public const string OutputcaheLatestappslistApps = @"/Partials/LatestAppsList";
        public const string OutputcaheTopappslistApps = @"/Partials/TopAppsList";
        

        #endregion

        #region Caching Data In file Locations

        public const string AppVirtualFieldsSavingAdditionalpath = @"Database\";
        public const string AppSuggestedAdditionalpath = @"Suggested\";
        public const string AppSearchResultsAdditionalpath = @"SearchResult\";

        #endregion

        #region Apps Already Found
        /// <summary>
        /// Will contain the recent app which is stored from App-Details page tap
        /// Controller : App
        /// Action : SingleAppDisplay
        /// Url : /Apps/Apple-7/Games/plant-vs-zombies
        /// </summary>
        public static List<App> StaticAppsList { get; set; }

        #endregion

        #region Reg expressions
        public const string FriendlyUrlRegex = @"[^A-Za-z0-9_\.~]+";
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

        public const string AppSavingExtension = @".mdb";

        #endregion

        #region Location of Images in Gallery Constants

        public const string AdditionalRootAdvertiseLocation = "Advertise/";

        /// <summary>
        /// "Gallery/"
        /// </summary>
        public const string AdditionalRootGalleryLocation = "Gallery/";
        /// <summary>
        /// "GalleryThumbs/"
        /// </summary>
        public const string AdditionalRootGalleryIconLocation = "GalleryThumbs/";
        /// <summary>
        /// "SearchThumbs/"
        /// </summary>
        public const string AdditionalRootSearchIconLocation = "SearchThumbs/";
        /// <summary>
        /// "HomePageThumbs/"
        /// </summary>
        public const string AdditionalRootHomeIconLocation = "HomePageThumbs/";
        /// <summary>
        /// "HomePageFeatured/"
        /// </summary>
        public const string AdditionalRootHomeLocation = "HomePageFeatured/";
        /// <summary>
        /// "SuggestionThumbs/"
        /// </summary>
        public const string AdditionalRootSuggestedIconLocation = "SuggestionThumbs/";

        /// <summary>
        /// "YoutubeCovers/"
        /// </summary>
        public const string YouTubeCoverImageLocation = "YoutubeCovers/";
        #endregion

        #region Apps Suggestions Number
        public const int SuggestHighestTake = 10;
        public const int SuggestHighestDisplayNumberSuggestions = 12;
        public const int SuggestHighestFromSameUser = 3;
        public const int SuggestHighestSameAppName = 3;
        public const int SuggestHighestAndSimilarQuery = 10;
        public const int SuggestHighestOrSimilarQuery = 10;
        public const int SearchResultsMaxResultReturn = 200;
        #endregion

        #region Truncate Len
        public const int AppHomePageDescriptionTruncLen = 45;
        
        #endregion

        #region Cache in file max expire time

        public const int AppSearchResultsExpireInHours = 36;
        public const int AppSuggestedResultsExpireInHours = 36;

        #endregion
    }
}