using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WereViewApp.Modules.Reponse {
    public class RemoveETagModule : IHttpModule {
        public void Dispose() { }

        public void Init(HttpApplication context) {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
            context.RequestCompleted += OnRequestComplete;
            context.PostRequestHandlerExecute += OnPostSendRequestHeaders;

        }

         void OnRequestComplete(object sender, EventArgs e) {
 
        }

        void OnPostSendRequestHeaders(object sender, EventArgs e) {
             HttpContext.Current.Response.Headers.Remove("ETag");
             //HttpContext.Current.Response.Headers.Remove("Cookie");
             HttpContext.Current.Response.Headers.Remove("X-Powered-By");
             HttpContext.Current.Response.Headers.Remove("Server");
             HttpContext.Current.Response.Headers.Remove("X-SourceFiles");

             HttpContext.Current.Request.Headers.Remove("ETag");
             //HttpContext.Current.Request.Headers.Remove("Cookie");
             HttpContext.Current.Request.Headers.Remove("X-Powered-By");
             HttpContext.Current.Request.Headers.Remove("Server");
             HttpContext.Current.Request.Headers.Remove("X-SourceFiles");
        }

        void OnPreSendRequestHeaders(object sender, EventArgs e) {

          
        }
    }
}