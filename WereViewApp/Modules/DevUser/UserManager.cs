using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WeReviewApp.BusinessLogics;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.POCO.Identity;
using WeReviewApp.Models.POCO.IdentityCustomization;
using WeReviewApp.Models.ViewModels;
using WeReviewApp.Modules.Role;
using WeReviewApp.Modules.Session;

namespace WeReviewApp.Modules.DevUser {
    public static class UserManager {
        #region Authentication

        public static bool IsAuthenticated() {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        #endregion

        #region Registration Code

        public static void LinkUserWithRegistrationCode(ApplicationUser user, Guid code) {
            if (user != null) {
                var relation = new RegisterCodeUserRelation { UserID = user.Id, RegisterCodeUserRelationID = code };
                using (var db = new ApplicationDbContext()) {
                    db.RegisterCodeUserRelations.Add(relation);
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region Complete Registration

        /// <summary>
        ///     Also make the EmailConfirmed = IsRegistrationComplete = true;
        ///     If first user then add all the roles.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="getRoleFromRegistration">Try get the role from the registration</param>
        /// <param name="role">has more priority than 'getRoleFromRegistration'. Add also the lower priority roles.</param>
        public static void CompleteRegistration(long userId, bool getRoleFromRegistration, string role = null) {
            using (var db2 = new ApplicationDbContext()) {
                var user = db2.Users.Find(userId);
                if (!user.IsRegistrationComplete) {
                    RegistrationCustomCode.CompletionBefore(user, getRoleFromRegistration, role);
                    RegistrationCustomCode.CompletionBefore(userId, getRoleFromRegistration, role);
                    if (user != null) {
                        // here completion doesn't work
                        if (!AppConfig.Setting.IsFirstUserFound) {
                            // first user not found yet.
                            // first user is admin
                            // most likely this is the first user

                            #region First User Registrations

                            var getHigestPriority = db2.Roles.Min(n => n.PriorityLevel);
                            // getting the highest priority role.
                            var getHigestPriorityRole = db2.Roles.FirstOrDefault(n => n.PriorityLevel == getHigestPriority);
                            // add all the roles to the  user.
                            RoleManager.AddUnderlyingRoles(user, getHigestPriorityRole.Name);
                            using (var db3 = new DevIdentityDbContext()) {
                                var setting = db3.CoreSettings.FirstOrDefault();
                                setting.IsFirstUserFound = true;
                                db3.SaveChanges(setting);
                                AppConfig.RefreshSetting();
                            }

                            #endregion
                        } else {
                            if (getRoleFromRegistration) {
                                if (role != null) {
                                    // role is given in the parameter specifically.
                                    RoleManager.AddUnderlyingRoles(user, role);
                                } else {
                                    // role has been saved from the registration time.
                                    var appRole = RoleManager.ReturnRoleIdFromTempInfoAndRemoveTemp(userId);
                                    if (appRole != null) {
                                        RoleManager.AddUnderlyingRoles(user, appRole.Result.Name);
                                    }
                                }
                            }
                        }
                        user.IsRegistrationComplete = true;
                        user.EmailConfirmed = true;
                        db2.SaveChanges(); // saved registration complete

                        RegistrationCustomCode.CompletionAfter(user, getRoleFromRegistration, role);
                        //wereviewdb user created with same id
                        RegistrationCustomCode.CompletionAfter(userId, getRoleFromRegistration, role);
                    }
                }
            }
        }

        #endregion

        #region Save User

        /// <summary>
        ///     Change current user record.
        /// </summary>
        /// <returns></returns>
        public static bool UpdateUser(ApplicationUser user) {
            using (var db = new ApplicationDbContext()) {
                db.Entry(user).State = EntityState.Modified;
                var i = db.SaveChanges();
                if (i == 0) {
                    return false;
                }
                return true;
            }
        }

        #endregion

        #region Remove user from session.

        public static void RemoveSessionUsersSession() {
            HttpContext.Current.Session[SessionNames.LastUser] = null;
            HttpContext.Current.Session[SessionNames.User] = null;
        }

        #endregion

        #region Get Every User

        /// <summary>
        ///     Get all the list of users.
        /// </summary>
        /// <returns>Returns all stored users in the database.</returns>
        public static List<ApplicationUser> GetAllUsers() {
            return Manager.Users.ToList();
        }

        /// <summary>
        ///     Get all the list of users.
        /// </summary>
        /// <returns>Returns all stored users as IQueryable for pagination.</returns>
        public static IQueryable<ApplicationUser> GetAllUsersAsIQueryable() {
            return Manager.Users;
        }

        #endregion

        #region Declaration

        private static ApplicationUserManager _userManager;

        public static ApplicationUserManager Manager {
            get {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set { _userManager = value; }
        }

        #endregion

        #region Asynchronous Operation

        public static async Task<ApplicationUser> GetUserAsync(string userName, string password) {
            return await Manager.FindAsync(userName, password);
        }

        public static async Task<ApplicationUser> GetUserByEmailAsync(string email, string password) {
            var user = GetUserFromSessionByEmail(email);
            if (user == null) {
                //not found in cache
                user = await Manager.FindByEmailAsync(email);
                if (user != null) {
                    user = await Manager.FindAsync(user.UserName, password);
                    SaveUserInSession(user);
                    return user;
                }
                return null;
            }
            user = await Manager.FindAsync(user.UserName, password);
            return user;
        }

        #endregion

        #region Get User

        /// <summary>
        ///     Username and id is same in both databases.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static ApplicationUser GetUser(long userid) {
            var user = GetUserFromSession(userid);
            if (user == null) {
                user = Manager.FindById(userid);
                SaveUserInSession(user);
            }
            return user;
        }

        /// <summary>
        ///     Username and id is same in both databases.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ApplicationUser GetUser(string userName, string password) {
            var user = GetUserFromSession(userName);
            if (user == null) {
                user = Manager.Find(userName, password);
                SaveUserInSession(user);
            }
            return user;
        }

        /// <summary>
        ///     Username and id is same in both databases.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ApplicationUser GetUserByEmail(string email, string password) {
            var user = Manager.FindByEmail(email);
            return Manager.Find(user.UserName, password);
        }

        /// <summary>
        ///     Username and id is same in both databases.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ApplicationUser GetUserByEmail(string email) {
            return Manager.FindByEmail(email);
        }

        public static string GetCurrentUserName() {
            if (HttpContext.Current.User.Identity.IsAuthenticated) {
                return HttpContext.Current.User.Identity.Name;
            }
            return null;
        }

        /// <summary>
        ///     Username and id is same in both databases.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static ApplicationUser GetUser(string username) {
            var user = GetUserFromSession(username);
            if (user == null) {
                user = Manager.FindByName(username);
                SaveUserInSession(user);
            }
            return user;
        }

        public static ApplicationUser GetUserFromSession() {
            var userSession = HttpContext.Current.Session[SessionNames.User];
            if (userSession != null) {
                return (ApplicationUser)userSession;
            }
            userSession = HttpContext.Current.Session[SessionNames.LastUser];
            if (userSession != null) {
                return (ApplicationUser)userSession;
            }
            return null;
        }

        public static ApplicationUser GetUserFromSessionByEmail(string email) {
            var user = GetUserFromSession();
            if (user != null && user.Email != null && email != null && user.Email.ToLower() == email.ToLower()) {
                return user;
            }
            return null;
            //user will give invalid result, because it might the previous user which credentials doesn;t match.
        }

        public static ApplicationUser GetUserFromSession(string username) {
            var user = GetUserFromSession();
            if (user != null && user.Email != null && username != null &&
                user.UserName.ToLower().Equals(username.ToLower())) {
                return user;
            }
            return null;
        }

        public static ApplicationUser GetUserFromSession(long userId) {
            var user = GetUserFromSession();
            if (user != null && user.UserID == userId) {
                return user;
            }
            return null;
        }

        public static ApplicationUser GetUserFromViewModel(RegisterViewModel model) {
            var user = new ApplicationUser {
                UserName = Logics.GetAllUpperCaseTitle(model.UserName),
                FirstName = Logics.GetAllUpperCaseTitle(model.FirstName),
                LastName = Logics.GetAllUpperCaseTitle(model.LastName),
                Email = model.Email,
                //DateOfBirth = model.DateOfBirth,
                CreatedDate = DateTime.Now,
                EmailConfirmed = false,
                PhoneNumber = model.Phone,
                //CountryID = model.CountryID,
                //CountryLanguageID = model.CountryLanguageID,
                //UserTimeZoneID = model.UserTimeZoneID,
                IsRegistrationComplete = false,
                GeneratedGuid = Guid.NewGuid()
            };

            return user;
        }

        /// <summary>
        ///     Return current user in optimized fashion.
        ///     Username and id is same in both databases.
        /// </summary>
        /// <returns></returns>
        public static ApplicationUser GetCurrentUser() {
            var username = GetCurrentUserName();
            if (username != null) {
                var user = GetUserFromSession(username);
                if (user == null) {
                    user = GetUser(username);
                    SaveCurrentUser(user);
                    return user;
                }
                return user;
            }
            return null;
        }

        /// <summary>
        ///     Return current user in optimized fashion.
        ///     Returns -1 if not logged in.
        ///     Username and id is same in both databases.
        /// </summary>
        /// <returns>Returns -1 if not logged in.</returns>
        public static long GetLoggedUserId() {
            if (HttpContext.Current.User.Identity.IsAuthenticated) {
                //ApplicationUser user = null;
                var userid = (long?)HttpContext.Current.Session[SessionNames.UserID];
                if (userid != null) {
                    return (long)userid;
                }
                return GetCurrentUser().UserID;
            }
            return -1;
        }

        #endregion

        #region User Exist check with Email or Username

        public static bool IsUserNameExist(string username) {
            return Manager.Users.Any(n => n.UserName == username);
        }

        /// <summary>
        ///     Checks if is empty()
        ///     then validate using regular expression then try searching in the db.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool IsUserNameExistWithValidation(string username, out ApplicationUser user) {
            user = null;
            if (!string.IsNullOrWhiteSpace(username)) {
                const int max = 30;
                const int min = 3;
                const string userPattern = "^([A-Za-z]|[A-Za-z0-9_.]+)$";
                var regularExpressionValidation = Regex.IsMatch(username, userPattern, RegexOptions.Compiled) &&
                                                  username.Length >= min && username.Length <= max;
                if (regularExpressionValidation) {
                    user = GetUser(username);
                    return user != null;
                }
            }
            return false;
        }

        public static bool IsEmailExist(string email) {
            return Manager.Users.Any(n => n.Email == email);
        }

        public static bool IsEmailExistWithValidation(string email, out ApplicationUser user) {
            user = null;
            if (!string.IsNullOrWhiteSpace(email)) {
                const int max = 30;
                const int min = 3;
                const string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                var regularExpressionValidation = Regex.IsMatch(email, emailPattern, RegexOptions.Compiled) &&
                                                  email.Length >= min && email.Length <= max;
                if (regularExpressionValidation) {
                    user = Manager.Users.FirstOrDefault(n => n.Email == email);
                    return user != null;
                }
            }
            return false;
        }

        #endregion

        #region Save Current user into session.

        /// <summary>
        ///     Save only current user in session
        /// </summary>
        /// <param name="user"></param>
        public static void SaveCurrentUser(ApplicationUser user) {
            HttpContext.Current.Session[SessionNames.User] = user;
            if (user != null) {
                HttpContext.Current.Session[SessionNames.UserID] = user.UserID;
            }
        }

        /// <summary>
        ///     Save last queried user in session.
        /// </summary>
        /// <param name="user"></param>
        public static void SaveUserInSession(ApplicationUser user) {
            HttpContext.Current.Session[SessionNames.LastUser] = user;
        }
        /// <summary>
        /// Clear user from session SessionNames.UserID, SessionNames.LastUser, SessionNames.UserCache
        /// </summary>
        public static void ClearUserSessions() {
            SessionNames.RemoveKeys(new[] { SessionNames.UserID, SessionNames.LastUser, SessionNames.UserCache });
            GC.Collect();
        }

        #endregion
    }
}