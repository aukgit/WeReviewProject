using System.Linq;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Models.EntityModel.ExtenededWithCustomMethods {
    public static class CategoryExtend {
        private const string ControllerName = "Category";
        private const string AppControllerName = "Apps";

        /// <summary>
        ///     returns a url like "/Apps/Category/@category.Slug"
        ///     and then put it in the "app.AbsUrl"
        ///     /Apps/Category/@category.Slug
        /// </summary>
        /// <returns>Returns Url without website's address. No slash at the end.</returns>
        public static string GetUrl(this Category category) {
            if (category != null) {
                return "/" + AppControllerName + "/" + ControllerName + "/" + category.Slug;
            }
            return "";
        }

        /// <summary>
        ///     returns a url like "http://url/Apps/Category/@category.Slug"
        ///     and then put it in the "app.AbsUrl"
        ///     @appUrl/Apps/Category/@category.Slug
        /// </summary>
        /// <returns>Returns absolute url including website's address. Returns "" when null.</returns>
        public static string GetAbsoluteUrl(this Category category) {
            if (category != null) {
                return AppVar.Url + GetUrl(category);
            }
            return "";
        }

        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <returns>Return category string from cache data empty string if not found.</returns>
        public static string GetCategoryString(this Category originalCategory, App app) {
            var category = WereViewStatics.AppCategoriesCache.FirstOrDefault(n => n.CategoryID == app.CategoryID);
            if (category != null) {
                return category.CategoryName;
            }
            return "";
        }
    }
}