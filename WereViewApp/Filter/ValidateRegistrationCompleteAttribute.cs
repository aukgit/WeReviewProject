using System.Web.Mvc;
using WeReviewApp.Modules.Extensions;
using WeReviewApp.Modules.Extensions.IdentityExtension;

namespace WeReviewApp.Filter {
    public class ValidateRegistrationCompleteAttribute : ActionFilterAttribute {


        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var user = filterContext.HttpContext.User;
            if (user != null && !user.IsRegistrationComplete()) {
                filterContext.RedirectToActionIfDistinct("Verify", "Account", "");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
