using System.Web.Mvc;
using System.Web.Routing;

namespace WereViewApp {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("elmah");

            #region Login, Register, Authentication Additional Routes

            const string wereviewappControllers = "WereViewApp.Controllers";
            const string accountController = "Account";

            routes.MapRoute(
                   name: "RegisterConfig",
                   url: "Register",
                   defaults: new { controller = accountController, action = "Register", id = UrlParameter.Optional },
                   namespaces: new string[] { wereviewappControllers }
            );
            routes.MapRoute(
                name: "SignInConfig",
                url: "SignIn",
                defaults: new { controller = accountController, action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { wereviewappControllers }
            );
            routes.MapRoute(
                name: "LoginConfig",
                url: "Login",
                defaults: new { controller = accountController, action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { wereviewappControllers }
            );

            routes.MapRoute(
                name: "SignOut",
                url: "SignOut",
                defaults: new { controller = accountController, action = "SignOutProgrammatically" },
                namespaces: new string[] { wereviewappControllers }
            );

            routes.MapRoute(
                name: "LogOff",
                url: "LogOff",
                defaults: new { controller = accountController, action = "SignOutProgrammatically" },
                namespaces: new string[] { wereviewappControllers }
            );

            routes.MapRoute(
               name: "ExternalSigninConfig",
               url: "ExtSignin",
               defaults: new { controller = accountController, action = "ExternalLogin", id = UrlParameter.Optional },
               namespaces: new string[] { wereviewappControllers }
           );
            #endregion


            #region profile
            routes.MapRoute(
              name: "profile",
              url: "Profile/{username}/{page}",
              defaults: new { controller = "Profile", action = "GetProfile", page = UrlParameter.Optional },
              namespaces: new string[] { wereviewappControllers }
           );
            routes.MapRoute(
              name: "profile",
              url: "Profile/{page}",
              defaults: new { page = UrlParameter.Optional },
              namespaces: new string[] { wereviewappControllers }
           );
            #endregion

            #region Default Route
            routes.MapRoute(
                   name: "Direct",
                   url: "{action}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   namespaces: new string[] { wereviewappControllers }
            );

            routes.MapRoute(
                   name: "Default",
                   url: "{controller}/{action}/{id}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   namespaces: new string[] { wereviewappControllers }
            );
            #endregion

            #region App Routes

            routes.MapRoute(
               name: "Apps",
               url: "Apps",
               defaults: new { controller = "App", action = "Index" },
               namespaces: new string[] { wereviewappControllers }
             );

            routes.MapRoute(
               name: "SingleApp",
               url: "Apps/{platform}-{platformVersion}/{category}/{url}",
               defaults: new { controller = "App", action = "SingleAppDisplay", name = UrlParameter.Optional },
               namespaces: new string[] { wereviewappControllers }
            );

            routes.MapRoute(
               name: "platformSearched",
               url: "Apps/{platformName}",
               defaults: new { controller = "Apps", action = "Platform" },
               namespaces: new string[] { wereviewappControllers }
            );



            #endregion



        }
    }
}
