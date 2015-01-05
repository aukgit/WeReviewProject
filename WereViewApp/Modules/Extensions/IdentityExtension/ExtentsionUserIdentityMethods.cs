using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace WereViewApp.Modules.Extensions.IdentityExtension {
    public static class ExtentsionUserIdentityMethods {
        public static long GetUserID(this IIdentity identity) {
            return long.Parse(identity.GetUserId());
        }
    }
}