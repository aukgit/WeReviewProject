using System;
using System.Web.Mvc;
using WeReviewApp.Modules.Extensions;

namespace WeReviewApp.Filter {
    public class PreventDuplicateCallAttribute : ActionFilterAttribute {
        public string RedirectAction { get; set; }
        public string RedirectController { get; set; }
        public string RedirectArea { get; set; }
        public string RedirectUrl { get; set; }
        public string OnDuplicateViewName { get; set; }
        /// <summary>
        /// Duration by which the current action will be prevented.
        /// </summary>
        public string DurationInMinsToAcceptAction { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var shortRouteProfile = filterContext.GetShortRouteProfile();
            var sessionName = shortRouteProfile.GetCombined(".");
            var session = filterContext.HttpContext.Session;
            if (session != null) {
                var lastVist = session[sessionName] as DateTime?;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}