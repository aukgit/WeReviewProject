using System.Web.Mvc;
using WeReviewApp.Modules.DevUser;
using WeReviewApp.Modules.Extensions;

namespace WeReviewApp.Filter {
    public class HasMinimumRoleAttribute : ActionFilterAttribute {
        public string MinimumRole { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!string.IsNullOrEmpty(MinimumRole)) {
                var userCache = UserCache.GetNewOrExistingUserCache();
                if (!userCache.HasMinimumRole(MinimumRole)) {
                    filterContext.RedirectToActionIfDistinct("Verify", "Account", "");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}