using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeReviewApp.Models.POCO.Identity;
using WeReviewApp.Modules.Role;
using WeReviewApp.Modules.Session;

namespace WeReviewApp.Modules.DevUser {
    public class UserCache {
        private bool _isAdmin;
        private bool _isAdminRoleGenerated;

        /// <summary>
        ///     Creates a user cache from current logged in user.
        ///     If not user exist then User will be null.
        ///     Check null before evaluating.
        ///     Note: Constructor also calls GenerateRoles() method to generate cached roles.
        /// </summary>
        public UserCache() {
            GenerateRoles();
        }

        /// <summary>
        ///     Creates a user cache from current logged in user.
        ///     If not user exist then User will be null.
        ///     Check null before evaluating.
        /// </summary>
        /// <param name="rolesGenerate">True : Generates cache roles for the current user.</param>
        /// <param name="saveUserInCache">True : Saves current cache in the session.</param>
        public UserCache(bool rolesGenerate, bool saveUserInCache) {
            if (rolesGenerate) {
                GenerateRoles();
            } else {
                User = UserManager.GetCurrentUser();
                IsRoleGenerated = false;
            }
            if (saveUserInCache) {
                SaveUserCache(this);
            }
        }

        /// <summary>
        ///     On creation get all the user roles to the cache.
        ///     GenerateRoles() has been called on creation.
        /// </summary>
        public UserCache(ApplicationUser user) {
            GenerateRoles(user);
        }

        /// <summary>
        ///     Creates a user cache from given user.
        ///     If not user exist then User will be null.
        ///     Check null before evaluating.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rolesGenerate">True : Generates cache roles for the current user.</param>
        /// <param name="saveUserInCache">True : Saves current cache in the session.</param>
        public UserCache(ApplicationUser user, bool rolesGenerate, bool saveUserInCache) {
            if (rolesGenerate) {
                GenerateRoles(user);
            } else {
                User = user;
                IsRoleGenerated = false;
            }
            if (saveUserInCache) {
                SaveUserCache(this);
            }
        }

        /// <summary>
        ///     Creates a user cache from given user.
        ///     If not user exist then User will be null.
        ///     Check null before evaluating.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rolesGenerate">True : Generates cache roles for the current user.</param>
        public UserCache(ApplicationUser user, bool rolesGenerate) {
            if (rolesGenerate) {
                GenerateRoles(user);
            } else {
                User = user;
                IsRoleGenerated = false;
            }
        }

        public ApplicationUser User { get; set; }

        public string Username {
            get { return User.UserName; }
        }

        public long UserID {
            get { return User.UserID; }
        }

        public List<ApplicationRole> Roles { get; set; }

        public bool IsRoleGenerated { get; set; }

        public bool IsRegistrationComplete {
            get { return User.IsRegistrationComplete; }
        }

        public bool IsAdmin {
            get {
                if (_isAdminRoleGenerated) {
                    return _isAdmin;
                }
                _isAdmin = IsInRole(RoleNames.Admin);
                _isAdminRoleGenerated = true;
                return _isAdmin;
            }
            set { _isAdmin = value; }
        }

        public bool IsAuth {
            get { return HttpContext.Current.User.Identity.IsAuthenticated; }
        }

        /// <summary>
        ///     Compare and return true if UserCache user and given parameter is same with user's respective property.
        /// </summary>
        /// <param name="userCache"></param>
        /// <param name="userId"></param>
        /// <returns>Compare and return true if UserCache user and given parameter is same with user's respective property.</returns>
        public static bool IsValidUserCache(UserCache userCache, long userId) {
            return userCache != null && userCache.User != null && userCache.UserID == userId;
        }

        /// <summary>
        ///     Compare and return true if UserCache user and given parameter is same with user's respective property.
        /// </summary>
        /// <param name="userCache"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool IsValidUserCache(UserCache userCache, string username) {
            return userCache != null && userCache.User != null && userCache.Username == username;
        }

        /// <summary>
        ///     Compare and return true if UserCache user and given parameter is same with user's respective property.
        /// </summary>
        /// <param name="userCache"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsValidUserCache(UserCache userCache, ApplicationUser user) {
            return userCache != null && userCache.User != null && user != null && userCache.UserID == user.UserID;
        }

        /// <summary>
        ///     Compare and return true if UserCache user and given parameter is same with user's respective property.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsValidUserCache(ApplicationUser user) {
            return IsValidUserCache(this, user);
        }

        /// <summary>
        ///     Compare and return true if UserCache user and given parameter is same with user's respective property.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsValidUserCache(long userId) {
            return IsValidUserCache(this, userId);
        }

        /// <summary>
        ///     Compare and return true if UserCache user and given parameter is same with user's respective property.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsValidUserCache(string username) {
            return IsValidUserCache(this, username);
        }

        /// <summary>
        ///     Get from cache or Creates a user cache from given user.
        ///     If not user exist then User will be null.
        ///     Check null before evaluating.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rolesGenerate">True : Generates cache roles for the current user.</param>
        /// <param name="saveUserInCache">True : Saves current cache in the session.</param>
        public static UserCache GetNewOrExistingUserCache(ApplicationUser user, bool rolesGenerate = true,
            bool saveUserInCache = true) {
            var userCahe = GetUserCacheSession();
            if (userCahe == null || userCahe.IsValidUserCache(user) == false) {
                userCahe = new UserCache(user, rolesGenerate, saveUserInCache);
            }
            return userCahe;
        }

        /// <summary>
        ///     Get from cache or Creates a user cache from logged user.
        ///     If not user exist then User will be null.
        ///     Check null before evaluating.
        /// </summary>
        /// <param name="rolesGenerate">True : Generates cache roles for the current user.</param>
        /// <param name="saveUserInCache">True : Saves current cache in the session.</param>
        public static UserCache GetNewOrExistingUserCache(bool rolesGenerate = true, bool saveUserInCache = true) {
            return GetNewOrExistingUserCache(UserManager.GetCurrentUser(), rolesGenerate, saveUserInCache);
        }

        public bool IsInRole(string roleName) {
            ApplicationRole role;
            return IsInRole(roleName, out role);
        }

        public bool IsInRole(string roleName, out ApplicationRole role) {
            role = null;
            if (IsAuth && IsRegistrationComplete && IsRoleGenerated) {
                role = Roles.FirstOrDefault(n => n.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            }
            return role != null;
        }

        public bool HasMinimumRole(string roleName) {
            ApplicationRole role;
            if (IsInRole(roleName, out role)) {
                var rolePriority = role.PriorityLevel;
                return Roles.Any(n => n.PriorityLevel >= rolePriority);
            }
            return false;
        }

        public void GenerateRoles() {
            if (UserManager.IsAuthenticated()) {
                User = UserManager.GetCurrentUser();
                GenerateRoles(User);
            }
        }

        public void GenerateRoles(ApplicationUser user) {
            User = user;
            Roles = RoleManager.GetUserRolesAsApplicationRole(Username);
            if (Roles == null) {
                Roles = new List<ApplicationRole>(0);
            }
            IsRoleGenerated = true;
            GC.Collect();
        }

        /// <summary>
        ///     Get user cache from session.
        /// </summary>
        /// <returns></returns>
        public static UserCache GetUserCacheSession() {
            return HttpContext.Current.Session[SessionNames.UserCache] as UserCache;
        }

        /// <summary>
        ///     Set user cache in session.
        /// </summary>
        /// <param name="userCache"></param>
        public static void SaveUserCache(UserCache userCache) {
            HttpContext.Current.Session[SessionNames.UserCache] = userCache;
        }

        /// <summary>
        ///     Creates user cache from User and then save it in session.
        /// </summary>
        /// <param name="user"></param>
        public static void SaveUserCache(ApplicationUser user) {
            var userCache = new UserCache(user);
            SaveUserCache(userCache);
        }

        /// <summary>
        ///     Clear usercache object session.
        /// </summary>
        public static void ClearSession() {
            SessionNames.RemoveKey(SessionNames.UserCache);
        }

        /// <summary>
        ///     Clear user from session SessionNames.UserID, SessionNames.LastUser, SessionNames.UserCache
        /// </summary>
        public static void ClearAllSession() {
            UserManager.ClearUserSessions();
        }
    }
}