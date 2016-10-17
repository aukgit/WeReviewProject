using System;
using System.Web.Mvc;
using System.Web.Routing;
using ReviewApps.Modules.Type;

namespace ReviewApps.Modules.Extensions {
    public static class ActionExecutingContextExtension {
        #region Get Controller Action Descriptor

        public static ActionDescriptor GetControllerActionDescriptor(this ActionExecutingContext context,
            string actionName) {
            if (context.ActionDescriptor != null &&
                context.ActionDescriptor.ControllerDescriptor != null) {
                return context.ActionDescriptor.ControllerDescriptor.FindAction(context.Controller.ControllerContext,
                    actionName);
            }
            return null;
        }

        #endregion

        #region Generate Parameters profile.

        /// <summary>
        ///     Can return null if HttpContext is null.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return ParametersProfileBase or null if HttpContext is null.</returns>
        public static ParametersProfileBase GetParametersProfile(this ActionExecutingContext context) {
            if (context.HttpContext != null) {
                return context.HttpContext.GetParametersProfile();
            }
            return null;
        }

        #endregion

        #region Getenerate Route profile

        /// <summary>
        ///     Generates a RouteProfile consist
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static RouteProfile GetRouteProfile(this ActionExecutingContext context) {
            return new RouteProfile(context);
        }

        /// <summary>
        ///     Generates a RouteProfile consist
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ShortRouteProfile GetShortRouteProfile(this ActionExecutingContext context) {
            return new ShortRouteProfile(context);
        }

        #endregion

        #region Property extension : Redirecting, Context null, Response empty , session empty etc..

        public static bool IsRedirecting(this ActionExecutingContext context) {
            return context != null &&
                   context.HttpContext != null &&
                   context.HttpContext.Response != null &&
                   context.HttpContext.Response.IsRequestBeingRedirected;
        }

        public static bool IsHttpContextEmpty(this ActionExecutingContext context) {
            return context.HttpContext == null;
        }

        public static bool IsResponseEmpty(this ActionExecutingContext context) {
            return context.HttpContext != null && context.HttpContext.Response == null;
        }

        public static bool IsRequestEmpty(this ActionExecutingContext context) {
            return context.HttpContext != null && context.HttpContext.Request == null;
        }

        public static bool IsSessionEmpty(this ActionExecutingContext context) {
            return context.HttpContext != null && context.HttpContext.Session == null;
        }

        #endregion

        #region Get : Action, Area, Controller names

        public static string GetAreaName(this ActionExecutingContext context) {
            return (string) context.RouteData.DataTokens["area"];
        }

        public static string GetControllerName(this ActionExecutingContext context) {
            return context.ActionDescriptor.ControllerDescriptor.ControllerName;
        }

        public static ControllerBase GetController(this ActionExecutingContext context) {
            return context.Controller;
        }

        public static string GetActionName(this ActionExecutingContext context) {
            return context.ActionDescriptor.ActionName;
        }

        #endregion

        #region Get : View Bag, View Data

        public static dynamic GetViewBag(this ActionExecutingContext context) {
            if (context.Controller != null) {
                return context.Controller.ViewBag;
            }
            return null;
        }

        public static ViewDataDictionary GetViewData(this ActionExecutingContext context) {
            if (context.Controller != null) {
                return context.Controller.ViewData;
            }
            return null;
        }

        #endregion

        #region Check/ Validate : IsAction, IsController, IsArea etc...

        /// <summary>
        ///     Returns true if the current action is same as the given name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actionName">Action name to test with current running action. Hint : context.ActionDescriptor.ActionName</param>
        /// <param name="onNullOrEmptyReturnDefault">Returns this default value if actionName parameter is null or empty string.</param>
        /// <returns>
        ///     Returns default value if parameter actionName is null or empty string. Or else returns only true if current
        ///     action is same as the given one.
        /// </returns>
        public static bool IsAction(this ActionExecutingContext context, string actionName,
            bool onNullOrEmptyReturnDefault = true) {
            if (string.IsNullOrEmpty(actionName)) {
                return onNullOrEmptyReturnDefault;
            }
            var action = context.ActionDescriptor.ActionName;
            return string.Equals(action, actionName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Returns true if the current area is same as the given name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="areaName">Area name to test with current area name.</param>
        /// <param name="onNullOrEmptyReturnDefault">Returns this default value if areaName parameter is null or empty string.</param>
        /// <returns>
        ///     Returns default value if parameter areaName is null or empty string. Or else returns only true if current area
        ///     is same as the given one.
        /// </returns>
        public static bool IsArea(this ActionExecutingContext context, string areaName,
            bool onNullOrEmptyReturnDefault = true) {
            if (string.IsNullOrEmpty(areaName)) {
                return onNullOrEmptyReturnDefault;
            }
            var value = (string) context.RouteData.DataTokens["area"];
            return string.Equals(value, areaName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Returns true if the current controller is same as the given name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="controllerName"></param>
        /// <param name="onNullOrEmptyReturnDefault">
        ///     Returns this default value if controllerName parameter is null or empty
        ///     string.
        /// </param>
        /// <returns>
        ///     Returns default value if controllerName parameter is null or empty string. Or else returns only true if
        ///     current area is same as the given one.
        /// </returns>
        public static bool IsController(this ActionExecutingContext context, string controllerName = null,
            bool onNullOrEmptyReturnDefault = true) {
            if (string.IsNullOrEmpty(controllerName)) {
                return onNullOrEmptyReturnDefault;
            }
            var value = context.ActionDescriptor.ControllerDescriptor.ControllerName;
            return string.Equals(value, controllerName, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Redirecting : Action, Area, Controller + Permanent Redirect.

        /// <summary>
        ///     Redirect to given action or url.
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
                        new RouteValueDictionary(new {controller, action, area})
                        );
                    context.Result.ExecuteResult(context.Controller.ControllerContext);
                }
            }
        }

        /// <summary>
        ///     Redirect to given action or url.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routeValueDictionary"></param>
        public static void RedirectTo(this ActionExecutingContext context, RouteValueDictionary routeValueDictionary) {
            var httpContext = context.HttpContext;
            if (!context.IsRedirecting()) {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(routeValueDictionary)
                    );
                context.Result.ExecuteResult(context.Controller.ControllerContext);
            }
        }

        /// <summary>
        ///     Permanent Redirect to given action or url. Status code : 301
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routeValueDictionary"></param>
        public static void RedirectPermanentTo(this ActionExecutingContext context,
            RouteValueDictionary routeValueDictionary) {
            var httpContext = context.HttpContext;
            if (!context.IsRedirecting()) {
                httpContext.Response.StatusCode = 301;
                httpContext.Response.Status = "301 Moved Permanently";
                RedirectTo(context, routeValueDictionary);
            }
        }

        /// <summary>
        ///     Permanent Redirect to given action or url. Status code : 301
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="controller">If not passed then current controller will be given.</param>
        /// <param name="area">If not passed then current area will be given.</param>
        public static void RedirectPermanentTo(this ActionExecutingContext context, string action, string controller,
            string area) {
            var httpContext = context.HttpContext;
            if (!context.IsRedirecting()) {
                if (!httpContext.Response.IsRequestBeingRedirected) {
                    httpContext.Response.StatusCode = 301;
                    httpContext.Response.Status = "301 Moved Permanently";
                    RedirectTo(context,
                        new RouteValueDictionary(new {
                            controller,
                            action,
                            area
                        }));
                }
            }
        }

        /// <summary>
        ///     Redirect to given action or url only if the action is not same as this.
        ///     If any redirect is happing then it will not execute.
        ///     Note : This will resolve the looping of redirection.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="controller">If not passed then current controller will be given.</param>
        /// <param name="area">If not passed then current area will be given.</param>
        public static void RedirectToActionIfDistinct(this ActionExecutingContext context, string action,
            string controller = null, string area = null) {
            if (!context.IsRedirecting()) {
                controller = string.IsNullOrEmpty(controller) ? GetControllerName(context) : controller;
                area = string.IsNullOrEmpty(area) ? GetAreaName(context) : area;
                var isCurrentUrlIsSameAs = IsCurrentUrl(context, action, controller, area);
                if (isCurrentUrlIsSameAs == false) {
                    RedirectTo(context, action, controller, area);
                }
            }
        }

        /// <summary>
        ///     Permanent Redirect to given action or url only if the action is not same as this.
        ///     If any redirect is happing then it will not execute.
        ///     Note : This will resolve the looping of redirection.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="controller">If not passed then current controller will be given.</param>
        /// <param name="area">If not passed then current area will be given.</param>
        public static void RedirectToActionPermanentIfDistinct(this ActionExecutingContext context, string action,
            string controller = null, string area = null) {
            if (!context.IsRedirecting()) {
                controller = string.IsNullOrEmpty(controller) ? GetControllerName(context) : controller;
                area = string.IsNullOrEmpty(area) ? GetAreaName(context) : area;
                var isCurrentUrlIsSameAs = IsCurrentUrl(context, action, controller, area);
                if (isCurrentUrlIsSameAs == false) {
                    RedirectPermanentTo(context, action, controller, area);
                }
            }
        }

        #endregion

        #region Check / Validate/ test : Current Url

        /// <summary>
        ///     Is Current action url is save as given ones in the parameter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        /// <param name="controller">If not passed then current controller will be given.</param>
        /// <param name="area">If not passed then current area will be given.</param>
        /// <returns></returns>
        public static bool IsCurrentUrl(this ActionExecutingContext context, string action, string controller = null,
            string area = null) {
            controller = string.IsNullOrEmpty(controller) ? GetControllerName(context) : controller;
            area = string.IsNullOrEmpty(area) ? GetAreaName(context) : area;
            var routeValues = new RouteValueDictionary(new {controller, action, area});
            return IsCurrentUrl(context, routeValues);
        }

        /// <summary>
        ///     Is Current action url is save as given ones in the parameter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routeValueDictionary"></param>
        /// <returns></returns>
        public static bool IsCurrentUrl(this ActionExecutingContext context, RouteValueDictionary routeValueDictionary) {
            var controller = (string) routeValueDictionary["controller"];
            var area = (string) routeValueDictionary["area"];
            var action = (string) routeValueDictionary["action"];
            var isActionSame = IsAction(context, action);
            var isControllerSame = IsController(context, controller);
            var isAreaSame = IsArea(context, area);

            return isActionSame &&
                   isControllerSame &&
                   isAreaSame;
        }

        #endregion
    }
}