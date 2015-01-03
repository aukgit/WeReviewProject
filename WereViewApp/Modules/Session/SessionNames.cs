using System.Web;
namespace WereViewApp.Modules.Session {
    public struct SessionNames {
        public const string User = "User";
        public const string UserID = "UserID";
        public const string LastUser = "LastSearchedUser";
        public const string IpAddress = "IpAddress";
        public const string Error = "GlobalCustomError";
        public const string Time = "PreviousTime";
        public const string Validator = "Validator";
        public const string ZoneInfo = "ZoneInfo";
        public const string AuthError = "AuthError";

        public static bool IsValidationExceed(string methodName, int maxTry = -1) {
            bool isSessionExist = HttpContext.Current.Session != null;

            if (maxTry == -1) {
                maxTry = AppConfig.ValidationMaxNumber;
            }

            string nameOfSession = SessionNames.Validator + methodName;
            var value = HttpContext.Current.Session[nameOfSession];
            if (isSessionExist && value != null) {
                int count = (int)value;
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
    }
}