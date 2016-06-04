using System;
using System.Web;
using DevMvcComponent.Extensions;

namespace WeReviewApp.Modules.Reponse {
    public class RemoveETagModule : IHttpModule {
        private static readonly string[] StaticExtensions = {
            ".js",
            ".css",
            ".jpg",
            ".png",
            ".bmp",
            ".json",
            ".json"
        };

        public void Dispose() {}

        public void Init(HttpApplication context) {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
            //context.RequestCompleted += OnRequestComplete;
            context.PostRequestHandlerExecute += OnPostSendRequestHeaders;
        }

        //private void OnRequestComplete(object sender, EventArgs e) {}

        private void OnPostSendRequestHeaders(object sender, EventArgs e) {
            HttpContext.Current.Response.Headers.Remove("ETag");
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("Server");

            HttpContext.Current.Request.Headers.Remove("ETag");
            //HttpContext.Current.Request.Headers.Remove("Cookie");
            HttpContext.Current.Request.Headers.Remove("X-Powered-By");
            HttpContext.Current.Request.Headers.Remove("Server");
            HttpContext.Current.Request.Headers.Add("Expires", "10000");
            //if (IsStaticContent(StaticExtensions)) {
            //    HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddYears(2));
            //    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            //    HttpContext.Current.Request.Cookies.Clear();
            //    HttpContext.Current.Request.Headers.Remove("Cookie");
            //    HttpContext.Current.Response.Headers.Remove("Cookie");
            //}
        }

        private bool IsStaticContent(string[] extensions) {
            var url = HttpContext.Current.Request.Url.ToString();
            var appUrlLength = AppVar.Url.Length - 2;
            foreach (var extension in extensions) {
                if (url.IsStringMatchfromLast(extension, appUrlLength)) {
                    return true;
                }
            }
            return false;
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e) {
            //HttpContext.Current.Request.Headers.Add("Expires", "100000");
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
        }
    }
}