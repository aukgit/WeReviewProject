using System.Web.Mvc;
using System.Web.Routing;
using WeReviewApp.Modules.Extensions.IdentityExtension;
using WeReviewApp.Modules.Extensions;

namespace WeReviewApp.Filter {
    public class ValidateRegistrationCompleteAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var user = filterContext.HttpContext.User;
            if (user.Identity.IsAuthenticated) {
                if (!user.IsRegistrationComplete()) {
                    filterContext.RedirectToActionIfDistinct("Verify", "Account", "");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
