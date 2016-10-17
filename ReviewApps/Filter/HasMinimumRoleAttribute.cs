using System.Web.Mvc;
using System.Web.Routing;
using ReviewApps.Modules.DevUser;
using ReviewApps.Modules.Extensions;

namespace ReviewApps.Filter {
    public class HasMinimumRoleAttribute : ActionFilterAttribute {
        public string MinimumRole { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var user = filterContext.HttpContext.User;
            if (user != null && user.Identity.IsAuthenticated) {
                if (!string.IsNullOrEmpty(MinimumRole)) {
                    var userCache = UserCache.GetNewOrExistingUserCache();
                    if (!userCache.HasMinimumRole(MinimumRole)) {
                        filterContext.RedirectToActionIfDistinct("Verify", "Account", "");
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}