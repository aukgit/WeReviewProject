#region using block
using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentScheduler;
using ReviewApps.Scheduler;

#endregion

namespace ReviewApps {
    public class MvcApplication : HttpApplication {
        protected void Application_Start() {

            //AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region Developers Organism Additional Settings in our Component

            AppConfig.RefreshSetting();

            #endregion

            MvcHandler.DisableMvcResponseHeader = true;
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            JobManager.Initialize(registry: new SchedulerRunner());
        }
        //protected void Application_BeginRequest(object sender, EventArgs e) {
        //    var app = sender as HttpApplication;
        //    if (app != null && app.Context != null) {
        //        //app.Context.Response.Headers.Remove("ETag");
        //        //app.Context.Response.Headers.Remove("X-Powered-By");
        //        //app.Context.Response.Headers.Remove("Server");
        //        //app.Context.Response.Headers.Remove("X-SourceFiles");
        //        //app.Context.Request.Headers.Remove("ETag");
        //        //app.Context.Request.Headers.Remove("X-Powered-By");
        //        //app.Context.Request.Headers.Remove("Server");
        //        //app.Context.Request.Headers.Remove("X-SourceFiles");
        //        //app.Context.Request.Headers.Add("Expires", "10000");
        //    }
        //}
        //protected void Application_EndRequest() {
        //    // removing excessive headers. They don't need to see this.
        //    //HttpContext.Current.Response.Headers.Remove("ETag");
        //    //HttpContext.Current.Response.Headers.Remove("X-Powered-By");
        //    //HttpContext.Current.Response.Headers.Remove("Server");
        //    //HttpContext.Current.Response.Headers.Remove("X-SourceFiles");

        //    //HttpContext.Current.Request.Headers.Remove("ETag");
        //    ////HttpContext.Current.Request.Headers.Remove("Cookie");
        //    //HttpContext.Current.Request.Headers.Remove("X-Powered-By");
        //    //HttpContext.Current.Request.Headers.Remove("Server");
        //    //HttpContext.Current.Request.Headers.Remove("X-SourceFiles");
        //    //HttpContext.Current.Request.Headers.Add("Expires", "10000");
        //}

        public override string GetVaryByCustomString(HttpContext context, string arg) {
            if (arg != null && arg.Equals("byuser", StringComparison.OrdinalIgnoreCase) ||
                arg.Equals("user", StringComparison.OrdinalIgnoreCase)) {
                //HttpCookie cookie = context.Request.Cookies["ASP.NET_SessionID"];
                //if (cookie != null) {
                //    return cookie.Value.ToString();
                //    //} else {
                //    //    return "const-none";
                //}
                return User.Identity.Name;
            }
            return base.GetVaryByCustomString(context, arg);
        }
    }
}