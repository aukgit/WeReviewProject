using System.Collections.Generic;
using System.Linq;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Models.EntityModel.ExtenededWithCustomMethods {
    public static class AppExtend {
        private const string ControllerNameForapp = "Apps";

        /// <summary>
        ///     returns a url like "http://url/Apps/iOs-7/Games/Plant-Vs-Zombies"
        ///     and then put it in the "app.AbsUrl"
        ///     /App/Platform-PlatformVersion/Category/Url
        /// </summary>
        /// <param name="app"></param>
        /// <returns>Returns absolute url including website's address</returns>
        public static string GetAbsoluteUrl(this App app) {
            if (app != null) {
                if (app.AbsUrl == null) {
                    var returnUrl = "/" + ControllerNameForapp + "/" + app.GetPlatformString() + "-" +
                                    app.PlatformVersion + "/" + app.GetCategorySlugString() + "/" + app.URL;
                    app.AbsUrl = AppVar.Url + returnUrl;
                }
                return app.AbsUrl;
            }
            return null;
        }


        public static ReviewLikeDislike GetCurrentUserReviewLikeDislike(this App app, Review review, long userId) {
            if (app.ReviewLikeDislikesCollection != null && review != null) {
                return app.ReviewLikeDislikesCollection
                    .FirstOrDefault(
                        n => n.ReviewID == review.ReviewID &&
                        n.UserID == userId);
            }
            return null;
        }

        /// <summary>
        /// "App/Platform-PlatformVersion/Category/Url"
        /// </summary>
        /// <param name="app"></param>
        /// <returns>Returns url without hostname and there is no slash front or end.</returns>
        public static string GetAppUrlWithoutHostName(this App app) {
            if (app != null) {
                var returnUrl = app.GetPlatformString() + "-" +
                                app.PlatformVersion + "/" + app.GetCategorySlugString() + "/" + app.URL;
                return returnUrl;
            }
            return null;
        }

        /// <summary>
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
        /// </summary>
        /// <param name="app"></param>
        /// <returns>Return category slug string from cache data empty string if not found.</returns>
        public static string GetCategorySlugString(this App app) {
            var category = WereViewStatics.AppCategoriesCache.FirstOrDefault(n => n.CategoryID == app.CategoryID);
            if (category != null) {
                return category.Slug;
            }
            return "";
        }
        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <returns>Return category from cache data.</returns>
        public static Category GetCategory(this App app) {
            return WereViewStatics.AppCategoriesCache.FirstOrDefault(n => n.CategoryID == app.CategoryID);
        }

        /// <summary>
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