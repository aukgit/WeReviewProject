using System;
using System.Web;
using System.Web.UI.WebControls;

namespace WeReviewApp.Modules.Reponse {
    public class RemoveETagModule : IHttpModule {
        public void Dispose() {}

        private static string[] _staticExtensions = {
            ".js",
            ".css",
            ".jpg",
            ".png",
            ".bmp",
            ".json",
            ".json",
        };

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
            HttpContext.Current.Response.Headers.Remove("X-SourceFiles");

            HttpContext.Current.Request.Headers.Remove("ETag");
            //HttpContext.Current.Request.Headers.Remove("Cookie");
            HttpContext.Current.Request.Headers.Remove("X-Powered-By");
            HttpContext.Current.Request.Headers.Remove("Server");
            HttpContext.Current.Request.Headers.Remove("X-SourceFiles");
            //HttpContext.Current.Request.Headers.Add("Expires","10000");
            if (IsStaticContent(_staticExtensions)) {
                HttpContext.Current.Request.Headers.Remove("Cookie");
                HttpContext.Current.Response.Headers.Remove("Cookie");
            }
        }

        private bool IsStaticContent(string [] extensions) {
            var url = HttpContext.Current.Request.Url.ToString();
            foreach (var extension in extensions) {
                var extLen = extension.Length;
                var isValid = true;
                var k = 0;
                for (int i = url.Length - 1; i < 0; i--) {
                    var index = extLen - 1 - k;
                    --k;
                    if (url[i] != extension[index]) {
                        isValid = false;
                        goto nextExtension;
                    }

                }
                nextExtension:
                if (isValid) {
                    return true;
                }
            }
            return false;
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e) {
            //HttpContext.Current.Request.Headers.Add("Expires", "100000");
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            if (IsStaticContent(_staticExtensions)) {
                HttpContext.Current.Request.Headers.Remove("Cookie");
                HttpContext.Current.Response.Headers.Remove("Cookie");
            }
        }
    }
}