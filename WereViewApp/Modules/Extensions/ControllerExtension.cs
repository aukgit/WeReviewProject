using System;
using System.Web.Mvc;
using System.Web.Routing;
using WeReviewApp.Modules.Type;

namespace WeReviewApp.Modules.Extensions {
    public static class ControllerExtension {

        #region Property extension : Redirecting, Context null, Response empty , session empty etc..

        public static bool IsRedirecting(this Controller context) {
            return context != null &&
                   context.HttpContext != null &&
                   context.HttpContext.Response != null &&
                   context.HttpContext.Response.IsRequestBeingRedirected;
        }

        public static bool IsHttpContextEmpty(this Controller context) {
            return context.HttpContext == null;
        }

        public static bool IsResponseEmpty(this Controller context) {
            return context.HttpContext != null && context.HttpContext.Response == null;
        }

        public static bool IsRequestEmpty(this Controller context) {
            return context.HttpContext != null && context.HttpContext.Request == null;
        }

        public static bool IsSessionEmpty(this Controller context) {
            return context.HttpContext != null && context.HttpContext.Session == null;
        }

        #endregion

        #region Get : Action, Area, Controller names

        public static string GetAreaName(this Controller context) {
            return (string) context.RouteData.DataTokens["area"];
        }

        public static string GetControllerName(this Controller context) {
            return context.ControllerContext.RouteData.Values["controller"].ToString();
        }

        public static string GetActionName(this Controller context) {
            return context.ControllerContext.RouteData.Values["action"].ToString();
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
        public static bool IsAction(this Controller context, string actionName,
            bool onNullOrEmptyReturnDefault = true) {
            if (string.IsNullOrEmpty(actionName)) {
                return onNullOrEmptyReturnDefault;
            }
            var action = GetActionName(context);
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
        public static bool IsArea(this Controller context, string areaName,
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
        public static bool IsController(this Controller context, string controllerName = null,
            bool onNullOrEmptyReturnDefault = true) {
            if (string.IsNullOrEmpty(controllerName)) {
                return onNullOrEmptyReturnDefault;
            }
            var value = GetControllerName(context);
            return string.Equals(value, controllerName, StringComparison.OrdinalIgnoreCase);
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
        public static bool IsCurrentUrl(this Controller context, string action, string controller = null,
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
        public static bool IsCurrentUrl(this Controller context, RouteValueDictionary routeValueDictionary) {
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