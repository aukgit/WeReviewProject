using System.Web.Mvc;
using WeReviewApp.Modules.Extensions;

namespace WeReviewApp.Filter {
    public class RedirectIfAlreadyLoggedInAttribute : ActionFilterAttribute {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (string.IsNullOrEmpty(Controller)) {
                Controller = "";
            }
            if (string.IsNullOrEmpty(Action)) {
                Action = "";
            }
            if (string.IsNullOrEmpty(Area)) {
                Area = "";
            }
            var user = filterContext.HttpContext.User;
            if (user != null && user.Identity.IsAuthenticated) {
                filterContext.RedirectToActionIfDistinct(Action, Controller, Area);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}