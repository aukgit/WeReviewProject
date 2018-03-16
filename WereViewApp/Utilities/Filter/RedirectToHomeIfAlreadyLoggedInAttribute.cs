using System.Web.Mvc;
using WeReviewApp.Modules.Extensions;

namespace WeReviewApp.Filter {
    /// <summary>
    /// Redirect to home if already logged in.
    /// </summary>
    public class RedirectToHomeIfAlreadyLoggedInAttribute : RedirectIfAlreadyLoggedInAttribute {

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (string.IsNullOrEmpty(Controller)) {
                Controller = "Home";
            }
            if (string.IsNullOrEmpty(Action)) {
                Action = "Index";
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