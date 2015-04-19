using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.POCO;
using WereViewApp.Modules.Cache;
using WereViewApp.Modules.Uploads;
using WereViewApp.WereViewAppCommon.Structs;

namespace WereViewApp.WereViewAppCommon {
    public static class WereViewStatics {



        static List<Category> _appCategoriesCache = null;
        static List<Platform> _appPlatformsCache = null;


        #region Declaration

        /// <summary>
        /// Icons means thumbs , "Gallery/"
        /// </summary>
        public static UploadProcessor uProcessorGallery = new UploadProcessor(CommonVars.ADDITIONAL_ROOT_GALLERY_LOCATION);
        /// <summary>
        /// Icons means thumbs , "GalleryThumbs/"
        /// </summary>
        public static UploadProcessor uProcessorGalleryIcons = new UploadProcessor(CommonVars.ADDITIONAL_ROOT_GALLERY_ICON_LOCATION);
        /// <summary>
        /// Icons means thumbs , "SearchThumbs/"
        /// </summary>
        public static UploadProcessor uProcessorSearchIcons = new UploadProcessor(CommonVars.ADDITIONAL_ROOT_SEARCH_ICON_LOCATION);
        /// <summary>
        /// Icons means thumbs , "HomePageThumbs/"
        /// </summary>
        public static UploadProcessor uProcessorHomeIcons = new UploadProcessor(CommonVars.ADDITIONAL_ROOT_HOME_ICON_LOCATION);
        /// <summary>
        /// "HomePageFeatured/"
        /// </summary>
        public static UploadProcessor uProcessorHomeFeatured = new UploadProcessor(CommonVars.ADDITIONAL_ROOT_HOME_LOCATION);
        /// <summary>
        /// Icons means thumbs ,  "SuggestionThumbs/"
        /// </summary>
        public static UploadProcessor uProcessorSuggestionIcons = new UploadProcessor(CommonVars.ADDITIONAL_ROOT_SUGGESTED_ICON_LOCATION);

        /// <summary>
        /// Icons means thumbs ,  "Advertise/"
        /// </summary>
        public static UploadProcessor uProcessorAdvertiseImages = new UploadProcessor(CommonVars.ADDITIONAL_ROOT_ADVERTISE_LOCATION);


        #endregion

        #region Get Notification type From Cache


        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId">NotificationTypeIDs.AppPost</param>
        /// <returns></returns>
        public static NotificationType GetNotificationType(byte taskId) {
            var type = CommonVars.NotificationTypesCache.FirstOrDefault(n => n.NotificationTypeID == taskId);

            return type;

        }
        #endregion
        #region Get User Point Values From Cache
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="taskId">UserPointsSettingIDs.AppPost</param>
        /// <returns></returns>
        public static UserPointSetting GetUserSettingPoint(byte taskId) {
            var point = CommonVars.UserPointSettingsCache.FirstOrDefault(n => n.UserPointSettingID == taskId);

            return point;

        }
        #endregion

        public static List<Category> AppCategoriesCache {
            get {
                if (_appCategoriesCache != null) {
                    return _appCategoriesCache;
                } else {
                    using (var db = new WereViewAppEntities()) {
                        _appCategoriesCache = db.Categories.ToList();
                    }
                }
                return _appCategoriesCache;
            }
            set {
                _appCategoriesCache = value;
            }
        }



        public static List<Platform> AppPlatformsCache {
            get {
                if (_appPlatformsCache != null) {
                    return _appPlatformsCache;
                } else {
                    using (var db = new WereViewAppEntities()) {
                        _appPlatformsCache = db.Platforms.ToList();
                    }
                }
                return _appPlatformsCache;
            }
        }
        /// <summary>
        /// Remove caches of platforms and categories.
        /// </summary>
        public static void RefreshCaches() {
            _appCategoriesCache = null;
            _appPlatformsCache = null;
        }

        static string getAppLocation(AppSavingTextFields app) {
            if (app != null && app.UploadGuid != null) {
                return app.UploadGuid.ToString() + "-appInfo.mdb";
            }
            return null;
        }
        static string getAppLocation(Guid UploadGuid) {
            if (UploadGuid != null) {
                return UploadGuid.ToString() + "-appInfo.mdb";
            }
            return null;
        }
        public static void SavingAppInDirectory(AppSavingTextFields app) {
            var textCache = new CacheDataInFile(CommonVars.APP_VIRTUAL_FIELDS_SAVING_ADDITIONALPATH);
            var location = getAppLocation(app);
            textCache.SaveInBinary(location, app);
        }
        public static AppSavingTextFields ReadAppFromDirectory(AppSavingTextFields app) {
            return ReadAppFromDirectory(app.UploadGuid);
        }
        public static AppSavingTextFields ReadAppFromDirectory(Guid uploadGuid) {
            var textCache = new CacheDataInFile(CommonVars.APP_VIRTUAL_FIELDS_SAVING_ADDITIONALPATH);
            var location = getAppLocation(uploadGuid);
            return (AppSavingTextFields)textCache.ReadObjectFromBinaryFile(location);
        }
    }
}