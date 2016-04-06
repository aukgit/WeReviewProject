using System.Web.Mvc;
using System.Web.Routing;
using WereViewApp.Modules.Extensions.IdentityExtension;

namespace WereViewApp.Filter {
    public class CheckRegistrationCompleteAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var user = filterContext.HttpContext.User;
            if (user.Identity.IsAuthenticated) {
                if (!user.IsRegistrationComplete()) {
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
