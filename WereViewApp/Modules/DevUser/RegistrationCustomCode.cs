using System.Web;
using WeReviewApp.Models.EntityModel;
using WeReviewApp.Models.POCO.Identity;

namespace WeReviewApp.Modules.DevUser {
    public static class RegistrationCustomCode {
        public static void CompletionBefore(long userId, bool getRoleFromRegistration, string role = null) {}

        public static void CompletionAfter(long userId, bool getRoleFromRegistration, string role = null) {
            UserManager.ClearUserSessions();
        }

        internal static void CompletionBefore(ApplicationUser userIndetity, bool getRoleFromRegistration, string role) {}

        internal static void CompletionAfter(ApplicationUser userIndetity, bool getRoleFromRegistration, string role) {
            using (var db = new WereViewAppEntities()) {
                var user = new User();
                user.UserID = userIndetity.UserID;
                user.FirstName = userIndetity.FirstName;
                user.Phone = userIndetity.PhoneNumber;
                user.LastName = userIndetity.LastName;
                user.UserName = userIndetity.UserName;
                db.Users.Add(user);
                if (db.SaveChanges(user) < 0) {
                    AppVar.Mailer.NotifyDeveloper(
                        "Can't save user in the WeReviewApp Database. Id maybe already present.",
                        "Can't save user in the WeReviewApp Database. Id maybe already present.", "Fatal Error");
                } else {
                    UserManager.ClearUserSessions();
                }
            }
        }
    }
}