using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Modules;
using WereViewApp.Modules.Claims;
//using WereViewApp.Modules.Garbage;


namespace WereViewApp {
    public partial class Startup {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app) {
            TestThingsStartup.BeforeLoadingMain();
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            #region Developers Organism Additional Settings in our Component

            AppConfig.RefreshSetting();

            #endregion

            #region Cookie Authentication
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/SignIn"),
                Provider = new CookieAuthenticationProvider {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, long>
                     (
                          validateInterval: TimeSpan.FromMinutes(30),
                          regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                          getUserIdCallback: (id) => (Int32.Parse(id.GetUserId()))
                     )
                }
            });
            #endregion

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            #region Facebook External Authentication

            if (AppConfig.Setting.IsFacebookAuthentication) {
                var facebookOptions = new FacebookAuthenticationOptions {
                      AppId = AppConfig.Setting.FacebookClientID.ToString(),
                      AppSecret = AppConfig.Setting.FacebookSecret,
                      Provider = new FacebookAuthenticationProvider() {
                          OnAuthenticated = (context) => {
                              context.Identity.AddClaim(new Claim("FacebookAccessToken", context.AccessToken));
                              return Task.FromResult(0);

                          }
                      },
                      SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
                      SendAppSecretProof = true
                };
                facebookOptions.Scope.Add("email user_photos");
                app.UseFacebookAuthentication(facebookOptions);
            }
            #endregion

            #region Commented out logins: Google, Twiter, Microsoft
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //}); 
            #endregion

            //ParsingNewProblems.SolveCountriesWhichDoesntHaveTimeZone();
            TestThingsStartup.AfterLoadingMain();
        }
    }
}