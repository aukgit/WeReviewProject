using System.Web;
using WeReviewApp.Models.POCO.Identity;
using WeReviewApp.Modules.Session;

namespace WeReviewApp.Models.ViewModels {
    public class EmailResendViewModel {

        private ApplicationUser _appUser;
        public string Email { get; set; }

        public ApplicationUser AppUser {
            get {
                return _appUser;

            }
            set {
                _appUser = value;
                _appUser.PasswordHash = "";
                Email = _appUser.Email;
            }
        }

        internal bool IsValid() {
            if (AppUser == null) {
                return false;
            }
            var model = (EmailResendViewModel)HttpContext.Current.Session[SessionNames.EmailResendViewModel];
            var email = AppUser.Email;
            if (model != null && model.AppUser != null && !string.IsNullOrWhiteSpace(email) && model.AppUser.Email.ToLower().Trim() == (email.ToLower().Trim())) {
                return true;
            }
            return false;
        }

        internal static bool IsValid(string email) {
            var model = (EmailResendViewModel)HttpContext.Current.Session[SessionNames.EmailResendViewModel];
            if (model == null) {
                return false;
            }
            var appUser = model.AppUser;
            if (appUser != null && !string.IsNullOrWhiteSpace(email) && appUser.Email != null &&
                appUser.Email.ToLower() == email.ToLower().Trim()) {
                return true;
            }
            return false;
        }

        internal static void SessionStore(EmailResendViewModel model) {
            HttpContext.Current.Session[SessionNames.EmailResendViewModel] = model;
        }
        internal static EmailResendViewModel GetEmailResendViewModelFromSession() {
            return (EmailResendViewModel)HttpContext.Current.Session[SessionNames.EmailResendViewModel];
        }
    }
}