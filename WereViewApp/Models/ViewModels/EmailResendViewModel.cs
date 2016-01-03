
using System.Web;
using WereViewApp.Modules.Session;

namespace WereViewApp.Models.ViewModels {
    public class EmailResendViewModel {
        public string Email { get; set; }

        internal bool IsValid() {
            var model = (EmailResendViewModel)HttpContext.Current.Session[SessionNames.EmailResendViewModel];
            if (model != null && !string.IsNullOrWhiteSpace(Email) && model.Email.ToLower().Trim() == (Email.ToLower().Trim())) {
                return true;
            }
            return false;
        }

        internal static bool IsValid(string email) {
            var model = (EmailResendViewModel)HttpContext.Current.Session[SessionNames.EmailResendViewModel];
            var newModel = new EmailResendViewModel() {
                Email = email
            };
            return newModel.IsValid();
        }

        internal static void SessionStore(EmailResendViewModel model) {
            HttpContext.Current.Session[SessionNames.EmailResendViewModel] = model;
        }
        internal static EmailResendViewModel GetEmailResendViewModelFromSession() {
            return (EmailResendViewModel)HttpContext.Current.Session[SessionNames.EmailResendViewModel];
        }
    }
}