#region using block
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WereViewApp.Scheduler;
using FluentScheduler;

#endregion

namespace WereViewApp {
    public class MvcApplication : HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region Developers Organism Additional Settings in our Component

            AppConfig.RefreshSetting();

            #endregion

            MvcHandler.DisableMvcResponseHeader = true;
            TaskManager.Initialize(registry: new SchedulerRunner());
        }

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