using System.Web;

namespace WereViewApp.Modules.Extensions {
    public static class UrlExtension {
        /// <summary>
        /// Returns BaseUrl and slash.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpContextBase request) {
            return GetBaseUrl(request);
        }

        /// <summary>
        /// Returns BaseUrl and slash.
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
        /// Returns BaseUrl and slash.
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