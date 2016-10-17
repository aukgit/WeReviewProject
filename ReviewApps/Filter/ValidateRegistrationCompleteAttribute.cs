using System.Web.Mvc;
using ReviewApps.Modules.Extensions;
using ReviewApps.Modules.Extensions.IdentityExtension;

namespace ReviewApps.Filter {
    public class ValidateRegistrationCompleteAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var user = filterContext.HttpContext.User;
            if (user != null && user.Identity.IsAuthenticated) {
                if (!user.IsRegistrationComplete()) {
                    filterContext.RedirectToActionIfDistinct("Verify", "Account", "");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
