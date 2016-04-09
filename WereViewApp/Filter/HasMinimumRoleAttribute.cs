using System.Web.Mvc;
using System.Web.Routing;
using WeReviewApp.Modules.DevUser;
using WeReviewApp.Modules.Role;

namespace WeReviewApp.Filter {
    public class HasMinimumRoleAttribute : ActionFilterAttribute {
        public string MinimumRole { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!string.IsNullOrEmpty(MinimumRole)) {
                if (!RoleManager.HasMiniumRole(MinimumRole)) {
                    filterContext.Result = new RedirectToRouteResult(
                         new RouteValueDictionary(new { controller = "Account", action = "Verify", area="" })
                     );

                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
