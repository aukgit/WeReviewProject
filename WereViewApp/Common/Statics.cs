using System;
using System.Collections.Generic;
using System.Linq;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.EntityModel;
using WeReviewApp.Models.POCO.IdentityCustomization;
using WeReviewApp.Modules.Cache;
using WeReviewApp.Modules.Uploads;

namespace WeReviewApp.Common {
    public static class WereViewStatics {
        private static List<Category> _appCategoriesCache;
        private static List<Platform> _appPlatformsCache;
        private static List<FeedbackCategory> _feedbackCategories;

        public static List<Category> AppCategoriesCache {
            get {
                if (_appCategoriesCache != null) {
                    return _appCategoriesCache;
                }
                using (var db = new WereViewAppEntities()) {
                    _appCategoriesCache = db.Categories.ToList();
                }
                return _appCategoriesCache;
            }
            set { _appCategoriesCache = value; }
        }

        public static List<FeedbackCategory> FeedbackCategories {
            get {
                if (_feedbackCategories != null) {
                    return _feedbackCategories;
                }
                using (var db = new ApplicationDbContext()) {
                    _feedbackCategories = db.FeedbackCategories.ToList();
                }
                return _feedbackCategories;
            }
            set { _feedbackCategories = value; }
        }

        public static List<Platform> AppPlatformsCache {
            get {
                if (_appPlatformsCache != null) {
                    return _appPlatformsCache;
                }
                using (var db = new WereViewAppEntities()) {
                    _appPlatformsCache = db.Platforms.ToList();
                }
                return _appPlatformsCache;
            }
        }

        #region Get Notification type From Cache

        /// <summary>
        /// </summary>
        /// <param name="taskId">NotificationTypeIDs.AppPost</param>
        /// <returns></returns>
        public static NotificationType GetNotificationType(byte taskId) {
            var type = CommonVars.NotificationTypesCache.FirstOrDefault(n => n.NotificationTypeID == taskId);

            return type;
        }

        #endregion

        /// <summary>
        ///     Remove caches of platforms and categories.
        /// </summary>
        public static void RefreshCaches() {
            _appCategoriesCache = null;
            _appPlatformsCache = null;
        }

        private static string getAppLocation(AppSavingTextFields app) {
            if (app != null && app.UploadGuid != null) {
                return app.UploadGuid + "-appInfo.mdb";
            }
            return null;
        }

        private static string getAppLocation(Guid uploadGuid) {
            if (uploadGuid != null) {
                return uploadGuid + "-appInfo.mdb";
            }
            return null;
        }

        public static void SavingAppInDirectory(AppSavingTextFields app) {
            var textCache = new CacheDataInFile(CommonVars.AppVirtualFieldsSavingAdditionalpath);
            var location = getAppLocation(app);
            textCache.SaveInBinary(location, app);
        }

        public static AppSavingTextFields ReadAppFromDirectory(AppSavingTextFields app) {
            return ReadAppFromDirectory(app.UploadGuid);
        }

        public static AppSavingTextFields ReadAppFromDirectory(Guid uploadGuid) {
            var textCache = new CacheDataInFile(CommonVars.AppVirtualFieldsSavingAdditionalpath);
            var location = getAppLocation(uploadGuid);
            return (AppSavingTextFields) textCache.ReadObjectFromBinaryFile(location);
        }

        #region Declaration

        /// <summary>
        ///     Icons means thumbs , "Gallery/"
        /// </summary>
        public static UploadProcessor UProcessorGallery = new UploadProcessor(CommonVars.AdditionalRootGalleryLocation);

        /// <summary>
        ///     Icons means thumbs , "GalleryThumbs/"
        /// </summary>
        public static UploadProcessor UProcessorGalleryIcons =
            new UploadProcessor(CommonVars.AdditionalRootGalleryIconLocation);

        /// <summary>
        ///     Icons means thumbs , "SearchThumbs/"
        /// </summary>
        public static UploadProcessor UProcessorSearchIcons =
            new UploadProcessor(CommonVars.AdditionalRootSearchIconLocation);

        /// <summary>
        ///     Icons means thumbs , "HomePageThumbs/"
        /// </summary>
        public static UploadProcessor UProcessorHomeIcons =
            new UploadProcessor(CommonVars.AdditionalRootHomeIconLocation);

        /// <summary>
        ///     "HomePageFeatured/"
        /// </summary>
        public static UploadProcessor UProcessorHomeFeatured = new UploadProcessor(CommonVars.AdditionalRootHomeLocation);

        /// <summary>
        ///     Icons means thumbs ,  "SuggestionThumbs/"
        /// </summary>
        public static UploadProcessor UProcessorSuggestionIcons =
            new UploadProcessor(CommonVars.AdditionalRootSuggestedIconLocation);

        /// <summary>
        ///     Icons means thumbs ,  "YoutubeCovers/"
        /// </summary>
        public static UploadProcessor UProcessorYoutubeCover = new UploadProcessor(CommonVars.YouTubeCoverImageLocation);

        /// <summary>
        ///     Icons means thumbs ,  "Advertise/"
        /// </summary>
        public static UploadProcessor UProcessorAdvertiseImages =
            new UploadProcessor(CommonVars.AdditionalRootAdvertiseLocation);

        #endregion

 
        #region Get User Point Values From Cache

        /// <summary>
        /// </summary>
        /// <param name="taskId">UserPointsSettingIDs.AppPost</param>
        /// <returns>If not found returns 0</returns>
        public static int GetUserSettingPointValue(byte taskId) {
            var point = CommonVars.UserPointSettingsCache.FirstOrDefault(n => n.UserPointSettingID == taskId);
            if (point != null) {
                return point.Point;
            }
            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="taskId">UserPointsSettingIDs.AppPost</param>
        /// <returns></returns>
        public static UserPointSetting GetUserSettingPoint(byte taskId) {
            var point = CommonVars.UserPointSettingsCache.FirstOrDefault(n => n.UserPointSettingID == taskId);

            return point;
        }
        #endregion

        #region Get all upload processors method
        /// <summary>
        /// Return all the upload processors
        /// </summary>
        /// <returns></returns>
        //public static UploadProcessor[] GetAllUploaderProcessor() {
        //    //if (_uploaderProcessors[0] == null) {
        //    //    _uploaderProcessors[0] = UProcessorGallery;
        //    //    _uploaderProcessors[1] = UProcessorGalleryIcons;
        //    //    _uploaderProcessors[2] = UProcessorSearchIcons;
        //    //    _uploaderProcessors[3] = UProcessorHomeIcons;
        //    //    _uploaderProcessors[4] = UProcessorHomeFeatured;
        //    //    _uploaderProcessors[5] = UProcessorSuggestionIcons;
        //    //    _uploaderProcessors[6] = UProcessorYoutubeCover;
        //    //    _uploaderProcessors[7] = UProcessorAdvertiseImages;
        //    //}
        //    //return _uploaderProcessors;
        //} 
        #endregion
    }
}