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

        public static bool IsRegistrationComplete(this IPrincipal user) {
            var userObject = GetUser(user);
            if (userObject != null) {
                return userObject.IsRegistrationComplete;
            }
            return false;
        }

        /// <summary>
        /// Returns -1 when user is not authenticated.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static long GetUserID(this IPrincipal user) {
            var userObject = GetUser(user);
            if (userObject != null) {
                return userObject.UserID;
            }
            return -1;
        }

        public static UserCache GetCahe(this IPrincipal user) {
            var userCache = UserCache.GetFromSession();
            if (userCache == null && user.Identity.IsAuthenticated) {
                userCache = new UserCache();
                UserCache.SetInSession(userCache);
                return userCache;
            }
            return null;
        }
    }
}