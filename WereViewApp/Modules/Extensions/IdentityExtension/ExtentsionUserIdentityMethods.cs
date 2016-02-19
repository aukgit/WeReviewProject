using System.Security.Principal;
using Microsoft.AspNet.Identity;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Modules.DevUser;

namespace WereViewApp.Modules.Extensions.IdentityExtension {
    public static class ExtentsionUserIdentityMethods {
        public static long GetUserID(this IIdentity identity) {
            return long.Parse(identity.GetUserId());
        }

        public static ApplicationUser GetUser(this IPrincipal user) {
            if (user.Identity.IsAuthenticated) {
                return UserManager.GetCurrentUser();
            }
            return null;
        }
        public static long GetUserID(this IPrincipal user) {
            if (user.Identity.IsAuthenticated) {
                var userObject = GetUser(user);
                return userObject.UserID;
            }
            return -1;
        }
    }
}