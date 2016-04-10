#region using block

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevMvcComponent.Error;
using FontAwesomeIcons;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WeReviewApp.Filter;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.POCO.Identity;
using WeReviewApp.Models.ViewModels;
using WeReviewApp.Modules.Constants;
using WeReviewApp.Modules.DevUser;
using WeReviewApp.Modules.Extensions.IdentityExtension;
using WeReviewApp.Modules.Mail;
using WeReviewApp.Modules.Role;
using WeReviewApp.Modules.Session;
using WeReviewApp.Modules.Validations;

#endregion

namespace WeReviewApp.Controllers {
    [Authorize]
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public class AccountController : Controller {
        #region Constructors

        public AccountController() {
            ViewBag.dynamicLoadPartialController = DynamicLoadPartialController;
            Manager = UserManager.Manager;
        }

        #endregion

        #region Call Complete Registration

        public void CallCompleteRegistration(long userId, string primaryRole = "Rookie") {
            UserManager.CompleteRegistration(userId, true, primaryRole);
        }

        #endregion

        #region LinkLogin

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider) {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        #endregion

        #region LinkLoginCallBack

        public async Task<ActionResult> LinkLoginCallback() {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null) {
                return RedirectToAction("Manage", new {Message = ManageMessageId.Error});
            }
            var result = await Manager.AddLoginAsync(User.Identity.GetUserID(), loginInfo.Login);
            if (result.Succeeded) {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new {Message = ManageMessageId.Error});
        }

        #endregion

        #region Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey) {
            ManageMessageId? message = null;
            var result =
                await Manager.RemoveLoginAsync(User.Identity.GetUserID(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded) {
                var user = await Manager.FindByIdAsync(User.Identity.GetUserID());
                await SignInAsync(user, false);
                message = ManageMessageId.RemoveLoginSuccess;
            } else {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new {Message = message});
        }

        #endregion

        #region External Login Fail

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure() {
            return View();
        }

        #endregion

        #region Remove Accounts List

        [ChildActionOnly]
        public ActionResult RemoveAccountList() {
            var linkedAccounts = Manager.GetLogins(long.Parse(User.Identity.GetUserId()));
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && Manager != null) {
                Manager.Dispose();

                Manager = null;
            }
            _db.Dispose();
            base.Dispose(disposing);
        }

        #region Check Inbox / InboxCheck

        [AllowAnonymous]
        public ActionResult Verify() {
            //var emailResender = EmailResendViewModel.GetEmailResendViewModelFromSession();
            //if (emailResender != null) {
            //    return View("InboxCheck");
            //} else {
            //    return AppVar.GetAuthenticationError("Not Authorized", "You have not logged in yet.");
            //}
            return View("InboxCheck");
        }

        #endregion

        #region Re-send Confirmation Email

        [Authorize]
        public async Task<ActionResult> ResendConfirmationMail() {
            var lastSend = Session["last-send"] as DateTime?;
            if (lastSend == null) {
                if (!User.IsRegistrationComplete()) {
                    var user = User.GetUser();
                    SendConfirmationEmail(user);
                    ViewBag.message =
                        "A verification email has been sent to your email address. Please check the spam folder if necessary.";
                    SignOutProgrammaticallyNonRedirect();
                } else {
                    ViewBag.message =
                        "Your registration is already complete! You have confirmed your account verification successfully.";
                    ViewBag.icon = FaIcons.CheckMark;
                }
            } else {
                ViewBag.message =
                    "You have already sent a verification code recently or your registration is complete.";
                ViewBag.icon = FaIcons.CheckMark;
            }
            Session["last-send"] = DateTime.Now;
            return View("InboxCheck");
        }

        #endregion

        #region ExternalLoginConfirm : External Register

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(RegisterViewModel model, string returnUrl) {
            if (User.Identity.IsAuthenticated) {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid) {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null) {
                    return View("ExternalLoginFailure");
                }
                var emailResender = EmailResendViewModel.GetEmailResendViewModelFromSession();

                if (emailResender != null) {
                    // that means user is already created successfully.
                    return RedirectToActionPermanent("Verify");
                }
                var user = UserManager.GetUserFromViewModel(model);
                var isUserExist = Manager.Users.Any(n => n.UserName == user.UserName);
                if (isUserExist == false) {
                    var result = await Manager.CreateAsync(user);
                    if (result.Succeeded) {
                        result = await Manager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded) {
                            if (AppVar.Setting.IsConfirmMailRequired) {
                                #region Send an email to the user about mail confirmation

                                SendConfirmationEmail(user);

                                #endregion

                                return RedirectToActionPermanent("Verify");
                            }
                            await SignInAsync(user, false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(result);
                } else {
                    // user already exist
                    return RedirectToActionPermanent("Verify");
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        #endregion

        #region Constants and Variable

        private const string ControllerName = "Account";

        /// Constant value for where the controller is actually visible.
        private const string DynamicLoadPartialController = "/Partials/";

        #endregion

        #region Confirm Email

        private async void SendConfirmationEmail(ApplicationUser user) {
            var code = Manager.GenerateEmailConfirmationToken(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                new {userId = user.Id, code, codeHashed = user.GeneratedGuid}, Request.Url.Scheme);
            var mailString = MailHtml.EmailConfirmHtml(user, callbackUrl);
            AppVar.Mailer.Send(user.Email, "Email Confirmation", mailString);
        }

        //[CompressFilter(Order = 1)]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code, Guid codeHashed) {
            if (code == null || codeHashed == null) {
                return View("Error");
            }
            var foundInUser = Guid.Empty;
            var result = await Manager.ConfirmEmailAsync(userId, code);
            var user = UserManager.GetUser(userId);
            if (user != null) {
                foundInUser = (Guid) user.GeneratedGuid;
            }
            if (!user.IsRegistrationComplete) {
                if (result.Succeeded && foundInUser.Equals(codeHashed)) {
                    CallCompleteRegistration(userId);
                    UserManager.ClearUserFromSession(); // clear user cache.
                    return View("ConfirmEmail");
                }
            } else {
                // already registered
                ViewBag.message = "You have already registered and confirmed your email successfully.";
                return View("InboxCheck");
            }
            AddErrors(result);
            return AppVar.GetFriendlyError("Confirmation is not valid.",
                "Sorry your confirmation is not valid. Please try again from /account/verify.");
        }

        #endregion

        #region Declaration

        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private PasswordHasher _passwordHasher = new PasswordHasher();
        public ApplicationUserManager Manager { get; private set; }

        #endregion

        #region Set ViewBag Objects

        public void SetRolesInViewBag() {
            if (AppVar.Setting.IsRegisterCodeRequiredToRegister) {
                ViewBag.Roles = new SelectList(RoleManager.GetRoles(), "Id", "Name");
            }
        }

        public void SetThingsInViewBag() {
            //ViewBag.Country = CachedQueriedData.GetCountries();
            //ViewBag.Country = CachedQueriedData.GetCountries();
        }

        #endregion

        #region Login

        private async Task SignInAsync(ApplicationUser user, bool isPersistent) {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties {IsPersistent = isPersistent},
                await user.GenerateUserIdentityAsync(Manager));
        }

        private void SignInProgrammatically(ApplicationUser user, bool isPersistent) {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = UserManager.Manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties {IsPersistent = isPersistent}, identity);
        }

        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            if (UserManager.IsAuthenticated()) {
                var userCache = User.GetNewOrExistingUserCache();
                if (userCache.IsRegistrationComplete) {
                    return RedirectToActionPermanent("Manage");
                }
                return RedirectToActionPermanent("Verify");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl) {
            if (ModelState.IsValid) {
                var user = await UserManager.GetUserByEmailAsync(model.Email, model.Password);
                if (user != null) {
                    await SignInAsync(user, model.RememberMe);
                    if (user.IsBlocked) {
                        SignOutProgrammaticallyNonRedirect();
                        return AppVar.GetAuthenticationError("You don't have the permission.",
                            "Sorry you don't have the permission to authenticate right now. Your account is blocked");
                    }
                    if (!user.IsRegistrationComplete) {
                        return RedirectToActionPermanent("Verify");
                    }
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError("", "Invalid username or password.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region LogOff

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult SignOut() {
            SignOutProgrammaticallyNonRedirect();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult SignOutProgrammatically() {
            SignOutProgrammaticallyNonRedirect();
            return RedirectToAction("Index", "Home");
        }

        public void SignOutProgrammaticallyNonRedirect() {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut();
        }

        #endregion

        #region Register

        [AllowAnonymous]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Register() {
            if (UserManager.IsAuthenticated()) {
                return AppVar.GetAuthenticationError("Authentication Failed", "");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[CompressFilter(Order = 1)]
        public async Task<ActionResult> Register(RegisterViewModel model) {
            var errors = new ErrorCollector();
            //External Validation.
            var validator = new DevUserValidator(model, errors, _db);
            var validOtherConditions = validator.ValidateEveryValidations();
            var emailResender = EmailResendViewModel.GetEmailResendViewModelFromSession();

            if (emailResender != null) {
                // that means user is already created successfully.
                return RedirectToActionPermanent("Verify");
            }

            if (ModelState.IsValid && validOtherConditions) {
                model.UserName = model.UserName.Trim();
                model.FirstName = model.FirstName.Trim();
                model.LastName = model.LastName.Trim();
                var user = UserManager.GetUserFromViewModel(model); // get user from view model.
                var result = await Manager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    // SignInProgrammatically(user, false);
                    // RoleManager.AddTempRoleInfo(user, model.Role);

                    if (AppVar.Setting.IsConfirmMailRequired && AppVar.Setting.IsFirstUserFound) {
                        // First user already found.
                        // mail needs to be confirmed and first user found.

                        #region Send an email to the user about mail confirmation

                        SendConfirmationEmail(user);

                        #endregion

                        #region Redirect to verify since registration

                        //SignOutProgrammaticallyNonRedirect();
                        return RedirectToActionPermanent("Verify");

                        #endregion
                    }
                    // first user not found or email doesn't need to be checked.
                    if (!AppVar.Setting.IsFirstUserFound) {
                        // first haven't found
                        // This is for first user.

                        #region Send an email to the user about mail confirmation

                        SendConfirmationEmail(user);

                        #endregion
                    }
                    CallCompleteRegistration(user.UserID, "Rookie"); // only will be called for first user.
                    return RedirectToActionPermanent("Verify");
                }
                AddErrors(result);
            }
            return View("Register", model);
        }

        #endregion

        #region External Logins

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl) {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        //
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl) {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await Manager.FindAsync(loginInfo.Login);
            if (user != null) {
                await SignInAsync(user, false);
                return RedirectToLocal(returnUrl);
            }
            // If the user does not have an account, then prompt the user to create an account
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            var fullName = loginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:facebook:name").Value;
            var fullNameArray = fullName.Split(' ').ToList();
            var lastName = fullNameArray.Last();
            fullNameArray.RemoveAt(fullNameArray.Count - 1);
            var firstName = string.Join(" ", fullNameArray);
            var username = fullName.Replace(" ", ".").ToLower();
            var accessToken =
                loginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "FacebookAccessToken").Value;
            var registerModel = new RegisterViewModel {
                Email = loginInfo.Email,
                FirstName = firstName,
                UserName = username,
                LastName = lastName,
                AccessToken = accessToken,
                Password = "HiddenPassword#!123--=",
                ConfirmPassword = "HiddenPassword#!123--="
            };

            return View("ExternalLoginConfirmation", registerModel);
        }

        #endregion

        #region Forget Password

        //[CompressFilter(Order = 1)]
        [AllowAnonymous]
        public ActionResult ForgotPassword() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[CompressFilter(Order = 1)]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {
            var isAlreadySent = !AppVar.IsInTestEnvironment && Session["forget-pass"] != null;
            if (!isAlreadySent) {
                if (ModelState.IsValid) {
                    var user = await Manager.FindByEmailAsync(model.Email);
                    if (user != null && await Manager.IsEmailConfirmedAsync(user.Id)) {
                        // valid user
                        SendResetPasswordLinkToUser(user);
                    }
                }
            } else {
                ViewBag.message = "You have had already sent a request just few seconds ago. Try again later.";
            }
            Session["forget-pass"] = "set";
            return View("ForgotPasswordConfirmation");
        }

        private async void SendResetPasswordLinkToUser(ApplicationUser user) {
            var code = Manager.GenerateUserToken(TokenPurpose.ResetPassword, user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account",
                new {userId = user.Id, email = user.Email, code, guid = user.GeneratedGuid}, Request.Url.Scheme);
            var mailString = MailHtml.PasswordResetHtml(user, callbackUrl);
            AppVar.Mailer.Send(user.Email, "Reset Password", mailString);
        }

        #endregion

        #region Password Reset

        [AllowAnonymous]
        public ActionResult ResetPassword(long userId, string email, string code, Guid guid) {
            var isAlreadySent = !AppVar.IsInTestEnvironment && Session["reset-pass"] != null;
            if (!isAlreadySent) {
                if (code == null || !Manager.VerifyUserToken(userId, TokenPurpose.ResetPassword, code)) {
                    return View("Error");
                }
                var user = User.GetUser(userId);
                if (user != null) {
                    if (string.Compare(email, user.Email, StringComparison.OrdinalIgnoreCase) == 0 &&
                        user.GeneratedGuid.HasValue &&
                        user.GeneratedGuid.Value == guid) {
                        User.SaveUserInSession(user, SessionNames.EmailResetExecute);
                        var model = new ResetPasswordViewModel {
                            Code = code,
                            Email = email
                        };
                        Session["reset-pass"] = "set";
                        return View(model);
                    }
                }
            }
            ViewBag.message = "You have already sent a request few minutes ago!";
            return View("ResetPasswordConfirmation");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateRegistrationComplete]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model) {
            if (Session["user-reset-" + model.Email] == null) {
                ApplicationUser user;
                if (User.IsUserExistInSessionByEmail(model.Email, out user, SessionNames.EmailResetExecute)) {
                    if (ModelState.IsValid) {
                        if (user == null) {
                            ModelState.AddModelError("", "No user found.");
                            return View();
                        }
                        var token = Manager.GeneratePasswordResetToken(user.Id);
                        var result = await Manager.ResetPasswordAsync(user.Id, token, model.Password);
                        if (result.Succeeded) {
                            Session["user-reset-" + model.Email] = "reset";
                            ViewBag.message = "Your account password has been reset successfully!";
                            return View("ResetPasswordConfirmation");
                        }
                        AddErrors(result);
                        return View(model);
                    }
                }
            }
            ViewBag.message = "Your had already reset your password just few minutes ago! Try again later.";
            return View("ResetPasswordConfirmation");
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation() {
            return View();
        }

        #endregion

        #region Account Manage

        [ValidateRegistrationComplete]
        public ActionResult Manage(ManageMessageId? message) {
            if (UserManager.IsAuthenticated()) {
                ViewBag.StatusMessage =
                    message == ManageMessageId.ChangePasswordSuccess
                        ? "Your password has been changed."
                        : message == ManageMessageId.SetPasswordSuccess
                            ? "Your password has been set."
                            : message == ManageMessageId.RemoveLoginSuccess
                                ? "The external login was removed."
                                : message == ManageMessageId.Error
                                    ? "An error has occurred."
                                    : "";
                ViewBag.HasLocalPassword = HasPassword();
                ViewBag.ReturnUrl = Url.Action("Manage");
                return View();
            }
            return SignOutProgrammatically();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateRegistrationComplete]
        public async Task<ActionResult> Manage(ManageUserViewModel model) {
            var hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword) {
                if (ModelState.IsValid) {
                    var result =
                        await
                            Manager.ChangePasswordAsync(User.Identity.GetUserID(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded) {
                        var user = await Manager.FindByIdAsync(User.Identity.GetUserID());
                        await SignInAsync(user, false);
                        return RedirectToAction("Manage", new {Message = ManageMessageId.ChangePasswordSuccess});
                    }
                    AddErrors(result);
                }
            } else {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                var state = ModelState["OldPassword"];
                if (state != null) {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid) {
                    var result = await Manager.AddPasswordAsync(User.Identity.GetUserID(), model.NewPassword);
                    if (result.Succeeded) {
                        return RedirectToAction("Manage", new {Message = ManageMessageId.SetPasswordSuccess});
                    }
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword() {
            var user = Manager.FindById(User.Identity.GetUserID());
            if (user != null) {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private class ChallengeResult : HttpUnauthorizedResult {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null) {}

            public ChallengeResult(string provider, string redirectUri, string userId) {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context) {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null) {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}