using System.Security.Principal;

namespace Microsoft.AspNet.Identity {
    public static class ExtentsionUserIdentityMethods {
        public static long GetUserID(this IIdentity identity) {
            return long.Parse(identity.GetUserId());
        }
    }
}