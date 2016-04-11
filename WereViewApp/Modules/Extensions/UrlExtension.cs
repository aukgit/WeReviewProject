using System.Web;
using System.Web.Mvc;

namespace WeReviewApp.Modules.Extensions {
    public static class UrlExtension {
        public static string CurrentControlerAbsoluteUrl(this Controller controller) {
            return AppVar.Url + HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        ///     Returns BaseUrl and slash.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpContextBase baseContext) {
            if (baseContext != null) {
                var request = baseContext.Request;
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
            }
            return "";
        }

        /// <summary>
        ///     Returns BaseUrl and slash.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpRequestBase request) {
            if (request.Url == null) {
                return "";
            }
            return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
        }

        /// <summary>
        ///     Returns BaseUrl and slash.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpContext context) {
            if (context != null) {
                var request = context.Request;
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~/");
            }
            return "";
        }
    }
}