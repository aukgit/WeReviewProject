using WeReviewApp.Models.Context;
using WeReviewApp.Models.POCO.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SendGrid;
using System.Net;
using System.Threading.Tasks;

namespace WeReviewApp {
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser, long> {

        #region Constructors
        public ApplicationUserManager(IUserStore<ApplicationUser, long> store)
            : base(store) {
        }
        #endregion

        #region Application UserManager Create
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) {
            var manager = new ApplicationUserManager(new ApplicatonUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, long>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser, long> {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser, long> {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null) {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, long>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
        #endregion
    }

    #region Services

    #region Email
    //public class EmailService : IIdentityMessageService {
    //    public void SendAsync(IdentityMessage message) {
    //        // Plug in your email service here to send an email.
    //        //await configSendGridasync(message);
    //    }

    //}  
    #endregion

    #region SMS
    //public class SmsService : IIdentityMessageService {
    //    public void SendAsync(IdentityMessage message) {

    //    }
    //} 
    #endregion

    #endregion


}
