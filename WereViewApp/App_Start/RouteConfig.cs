using System.Web.Mvc;
using System.Web.Routing;

namespace WereViewApp {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("elmah");

            #region Login, Register, Authentication Additional Routes
            routes.MapRoute(
                   name: "RegisterConfig",
                   url: "Register",
                   defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
                   namespaces: new string[] { "WereViewApp.Controllers" }
               );
            routes.MapRoute(
                name: "SignInConfig",
                url: "SignIn",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "WereViewApp.Controllers" }
            );
            routes.MapRoute(
                name: "LoginConfig",
                url: "Login",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "WereViewApp.Controllers" }
            );

            routes.MapRoute(
                name: "SignOut",
                url: "SignOut",
                defaults: new { controller = "Account", action = "SignOut", id = UrlParameter.Optional },
                namespaces: new string[] { "WereViewApp.Controllers" }
            );

            routes.MapRoute(
               name: "ExternalSigninConfig",
               url: "ExtSignin",
               defaults: new { controller = "Account", action = "ExternalLogin", id = UrlParameter.Optional },
               namespaces: new string[] { "WereViewApp.Controllers" }
           );
            #endregion



            #region Default Route
            routes.MapRoute(
                   name: "Direct",
                   url: "{action}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   namespaces: new string[] { "WereViewApp.Controllers" }
            );
        
            routes.MapRoute(
                   name: "Default",
                   url: "{controller}/{action}/{id}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   namespaces: new string[] { "WereViewApp.Controllers" }
            );
            #endregion

            #region App Routes

            routes.MapRoute(
               name: "Apps",
               url: "Apps",
               defaults: new { controller = "App", action = "Index" },
               namespaces: new string[] { "WereViewApp.Controllers" }
             );

            routes.MapRoute(
               name: "SingleApp",
               url: "App/{platform}-{platformVersion}/{category}/{url}",
               defaults: new { controller = "App", action = "SingleAppDisplay", name = UrlParameter.Optional },
               namespaces: new string[] { "WereViewApp.Controllers" }
            );

            routes.MapRoute(
               name: "platformSearched",
               url: "Apps/{platformName}",
               defaults: new { controller = "Apps", action = "Platform" },
               namespaces: new string[] { "WereViewApp.Controllers" }
            );



            #endregion


        }
    }
}
