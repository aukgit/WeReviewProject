using System.Web.Mvc;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Role;

namespace WereViewApp.Filter {
    public class HasMinimumRoleAttribute : ActionFilterAttribute {
        public string MinimumRole { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!string.IsNullOrEmpty(MinimumRole)) {
                if (!RoleManager.HasMiniumRole(MinimumRole)) {
                    return RedirectResult();
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
