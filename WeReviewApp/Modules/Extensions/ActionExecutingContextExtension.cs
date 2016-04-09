using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WeReviewApp.Modules.Type;

namespace WeReviewApp.Modules.Extensions {
    public static class ActionExecutingContextExtension {
        public static string GetAreaName(this ActionExecutingContext context) {
            return (string)context.RouteData.DataTokens["area"];
        }
        public static string GetControllerName(this ActionExecutingContext context) {
            return (string)context.RouteData.DataTokens["controller"];
        }

        public static ControllerBase GetController(this ActionExecutingContext context) {
            return context.Controller;
        }

        public static dynamic GetViewBag(this ActionExecutingContext context) {
            return context.Controller.ViewBag;
        }

        public static ViewDataDictionary GetViewData(this ActionExecutingContext context) {
            return context.Controller.ViewData;
        }


        public static string GetActionName(this ActionExecutingContext context) {
            return (string)context.RouteData.DataTokens["action"];
        }

        public static bool IsAction(this ActionExecutingContext context, string actionName) {
            var action = (string)context.RouteData.DataTokens["action"];
            return string.Equals(action, actionName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsArea(this ActionExecutingContext context, string areaName) {
            var value = (string)context.RouteData.DataTokens["area"];
            return string.Equals(value, areaName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsController(this ActionExecutingContext context, string controllerName) {
            var value = (string)context.RouteData.DataTokens["controller"];
            return string.Equals(value, controllerName, StringComparison.OrdinalIgnoreCase);
        }
        public static IDictionary<string, object> GetParameters(this ActionExecutingContext context) {
            return context.ActionParameters;
        }

        /// <summary>
        /// This will redirect to new url.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="area"></param>
        public static void RedirectTo(this ActionExecutingContext context, string action, string controller, string area) {
            var httpContext = context.HttpContext;
            if (httpContext != null && httpContext.Response != null) {
                if (!httpContext.Response.IsRequestBeingRedirected) {
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = controller, action = action, area = area })
                        );
                    context.Result.ExecuteResult(context.Controller.ControllerContext);
                }
            }
        }

        /// <summary>
        /// This will redirect to new url.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routeValueDictionary"></param>
        public static void RedirectTo(this ActionExecutingContext context, RouteValueDictionary routeValueDictionary) {
            var httpContext = context.HttpContext;
            if (httpContext != null && httpContext.Response != null) {
                if (!httpContext.Response.IsRequestBeingRedirected) {
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(routeValueDictionary)
                        );
                    context.Result.ExecuteResult(context.Controller.ControllerContext);
                }
            }
        }

        /// <summary>
        /// This will redirect to new url.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routeValueDictionary"></param>
        public static void RedirectPermanentTo(this ActionExecutingContext context, RouteValueDictionary routeValueDictionary) {
            var httpContext = context.HttpContext;
            if (httpContext != null && httpContext.Response != null) {
                if (!httpContext.Response.IsRequestBeingRedirected) {
                    httpContext.Response.StatusCode = 301;
                    httpContext.Response.Status = "301 Moved Permanently";
                    RedirectTo(context, routeValueDictionary);
                }
            }
        }

        /// <summary>
        /// This will redirect to new url.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="area"></param>
        public static void RedirectPermanentTo(this ActionExecutingContext context, string action, string controller, string area) {
            var httpContext = context.HttpContext;
            if (httpContext != null && httpContext.Response != null) {
                if (!httpContext.Response.IsRequestBeingRedirected) {
                    httpContext.Response.StatusCode = 301;
                    httpContext.Response.Status = "301 Moved Permanently";
                    RedirectTo(context,
                        new RouteValueDictionary(new { controller = controller, action = action, area = area }));
                }
            }
        }

        /// <summary>
        /// Redirect to url only the url is not same as this.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="area"></param>
        public static void RedirectToActionIfDistinct(this ActionExecutingContext context, string action, string controller, string area) {
            var isCurrentUrlIsSameAs = IsCurrentUrl(context, action, controller, area);
            if (isCurrentUrlIsSameAs == false) {
                RedirectTo(context, action, controller, area);
            }
        }

        /// <summary>
        /// Redirect to url only the url is not same as this.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="area"></param>
        public static void RedirectToActionPermanentIfDistinct(this ActionExecutingContext context, string action, string controller, string area) {
            var isCurrentUrlIsSameAs = IsCurrentUrl(context, action, controller, area);
            if (isCurrentUrlIsSameAs == false) {
                RedirectPermanentTo(context, action, controller, area);
            }
        }

        public static bool IsCurrentUrl(this ActionExecutingContext context, string action, string controller, string area) {
            var routeValues = new RouteValueDictionary(new { controller = controller, action = action, area = area });
            return IsCurrentUrl(context, routeValues);
        }

        public static bool IsCurrentUrl(this ActionExecutingContext context, RouteValueDictionary routeValueDictionary) {
            var controller = (string)routeValueDictionary["controller"];
            var area = (string)routeValueDictionary["area"];
            var action = (string)routeValueDictionary["action"];
            return (IsAction(context, action)) && (IsController(context, controller)) && (IsArea(context, area));
        }

        public static RouteProfile GetRouteProfile(this ActionExecutingContext context) {
            var routeProfile = new RouteProfile();
            var route = context.RouteData;
            routeProfile.Action = (string)route.DataTokens["action"];
            routeProfile.Controller = (string)route.DataTokens["controller"];
            routeProfile.Area = (string)route.DataTokens["area"];
            routeProfile.Parameters = context.ActionParameters;
            routeProfile.ActionDescriptor = context.ActionDescriptor;
            return routeProfile;
        }
        /// <summary>
        /// Can return null if HttpContext is null.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return ParametersProfileBase or null if HttpContext is null.</returns>
        public static ParametersProfileBase GetParametersProfile(this ActionExecutingContext context) {
            if (context.HttpContext != null) {
                return context.HttpContext.GetParametersProfile();
            }
            return null;
        }

    }
}