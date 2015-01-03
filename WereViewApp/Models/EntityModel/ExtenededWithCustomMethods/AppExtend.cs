using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Models.EntityModel.ExtenededWithCustomMethods {
    public static class AppExtend {
        const string _controllerNameForapp = "App";

        /// <summary>
        /// returns a url like "http://url/App/iOs-7/Games/Plant-Vs-Zombies"
        /// /App/Platform-PlatformVersion/Category/Url
        /// </summary>
        /// <param name="app"></param>
        /// <returns>Returns absolutue url including website's address</returns>
        public static string GetAppURL(this App app) {
            if (app != null) {
                if (app.AbsUrl == null) {
                    var returnURL = "/" + _controllerNameForapp + "/" + app.GetPlatformString() + "-" +
                                    app.PlatformVersion + "/" + app.GetCategoryString() + "/" + app.URL;
                    app.AbsUrl = AppVar.Url + returnURL;
                }
                return app.AbsUrl;
            }
            return null;
        }

        public static string GetAppURLWithoutHostName(this App app) {
            if (app != null) {
                var returnURL = app.GetPlatformString() + "-" +
                                app.PlatformVersion + "/" + app.GetCategoryString() + "/" + app.URL;
                return returnURL;
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns>Return category string from cache data empty string if not found.</returns>
        public static string GetCategoryString(this App app) {
            var category = WereViewStatics.AppCategoriesCache.FirstOrDefault(n => n.CategoryID == app.CategoryID);
            if (category != null) {
                return category.CategoryName;
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns>Return platform string from cache data empty string if not found.</returns>

        public static string GetPlatformString(this App app) {
            var platform = WereViewStatics.AppPlatformsCache.FirstOrDefault(n => n.PlatformID == app.PlatformID);
            if (platform != null) {
                return platform.PlatformName;
            }
            return "";
        }

        public static Platform GetPlatform(this App app) {
            var platform = WereViewStatics.AppPlatformsCache.FirstOrDefault(n => n.PlatformID == app.PlatformID);
            if (platform != null) {
                return platform;
            }
            return null;
        }

    }
}