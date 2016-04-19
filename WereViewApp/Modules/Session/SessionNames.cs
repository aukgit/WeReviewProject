using System.Web;

namespace WeReviewApp.Modules.Session {
    public static class SessionNames {
        public const string User = "User";
        public const string UserID = "UserID";
        public const string LastUser = "LastSearchedUser";
        public const string IpAddress = "IpAddress";
        public const string Error = "GlobalCustomError";
        public const string Time = "PreviousTime";
        public const string Validator = "Validator";
        public const string ZoneInfo = "ZoneInfo";
        public const string AuthError = "AuthError";
        public const string EmailResendViewModel = "EmailResendViewModel";
        public const string EmailResetExecute = "EmailResetExecute";
        public const string UserPrincipalUserSession = "User.Principal.User.Session";
        public const string UserPrincipalUserCacheSession = "User.Principal.User.Cache.Session";
        public const string UserCache = "UserCache.Session";
        public const string ForgetPasswordView = "ForgetPassword.view";

        public static bool IsValidationExceed(string methodName, int maxTry = -1) {
            var isSessionExist = HttpContext.Current.Session != null;

            if (maxTry == -1) {
                maxTry = AppConfig.ValidationMaxNumber;
            }

            var nameOfSession = Validator + methodName;
            var value = HttpContext.Current.Session[nameOfSession];
            if (isSessionExist && value != null) {
                var count = (int) value;
                if (count <= maxTry) {
                    HttpContext.Current.Session[nameOfSession] = ++count;
                    return false;
                }
            } else if (isSessionExist && value == null) {
                //when null
                HttpContext.Current.Session[nameOfSession] = 1;
                return false;
            }
            return true;
        }
        public static void RemoveKeys(string[] keys) {
            if (HttpContext.Current != null) {
                var session = HttpContext.Current.Session;
                if (session != null) {
                    foreach (var key in keys) {
                        session.Remove(key);
                    }
                }
            }
        }

        public static void RemoveKey(string key) {
            if (HttpContext.Current != null) {
                var session = HttpContext.Current.Session;
                if (session != null) {
                    session.Remove(key);
                }
            }
        }

    }
}