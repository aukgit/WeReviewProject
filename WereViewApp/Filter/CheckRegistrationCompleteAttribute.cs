using System.Web.Mvc;
using System.Web.Routing;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Role;

namespace WereViewApp.Filter {
    public class CheckRegistrationCompleteAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated) {
                var user = UserManager.GetCurrentUser();
                if (!user.IsRegistrationComplete) {
                    filterContext.Result = new RedirectToRouteResult(
                         new RouteValueDictionary(new { controller = "Account", action = "Verify", area = "" })
                     );
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
