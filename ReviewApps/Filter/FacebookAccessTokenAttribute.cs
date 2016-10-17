using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReviewApps.Modules.Extensions.IdentityExtension;
namespace ReviewApps.Filter {
    public class FacebookAccessTokenAttribute : ActionFilterAttribute {

        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            ApplicationUserManager userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (userManager != null) {
                var userId = filterContext.HttpContext.User.Identity.GetUserID();
                var claimsforUser = userManager.GetClaimsAsync(userId);
                var accessToken = claimsforUser.Result.FirstOrDefault(x => x.Type == "FacebookAccessToken").Value;

                if (filterContext.HttpContext.Items.Contains("access_token"))
                    filterContext.HttpContext.Items["access_token"] = accessToken;
                else
                    filterContext.HttpContext.Items.Add("access_token", accessToken);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}