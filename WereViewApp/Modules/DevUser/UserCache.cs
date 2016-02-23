using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Modules.Role;
namespace WereViewApp.Modules.DevUser {
    public class UserCache {
        /// <summary>
        /// Creates a user cache from current logged in user.
        /// If not user exist then User will be null.
        /// Check null before evaluating.
        /// Note: Constructor also calls GenerateRoles() method to generate cached roles.
        /// </summary>
        public UserCache() {
            GenerateRoles();
        }
        /// <summary>
        /// Creates a user cache from current logged in user.
        /// If not user exist then User will be null.
        /// Check null before evaluating.
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
                SetInSession(this);
            }
        }

        /// <summary>
        /// On creation get all the user roles to the cache.
        /// GenerateRoles() has been called on creation.
        /// </summary>
        public UserCache(ApplicationUser user) {
            GenerateRoles(user);
        }

        /// <summary>
        /// Creates a user cache from given user.
        /// If not user exist then User will be null.
        /// Check null before evaluating.
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
                SetInSession(this);
            }
        }

        /// <summary>
        /// Creates a user cache from given user.
        /// If not user exist then User will be null.
        /// Check null before evaluating.
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
        /// <summary>
        /// Get from cache or Creates a user cache from given user.
        /// If not user exist then User will be null.
        /// Check null before evaluating.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rolesGenerate">True : Generates cache roles for the current user.</param>
        /// <param name="saveUserInCache">True : Saves current cache in the session.</param>
        public UserCache GetNewOrExistingUserCache(ApplicationUser user, bool rolesGenerate, bool saveUserInCache) {
            var userCahe = UserCache.GetFromSession();
            if (userCahe == null) {
                userCahe = new UserCache(user, rolesGenerate, saveUserInCache);
            }
            return userCahe;
        }

        /// <summary>
        /// Get from cache or Creates a user cache from logged user.
        /// If not user exist then User will be null.
        /// Check null before evaluating.
        /// </summary>
        /// <param name="rolesGenerate">True : Generates cache roles for the current user.</param>
        /// <param name="saveUserInCache">True : Saves current cache in the session.</param>
        public UserCache GetNewOrExistingUserCache(bool rolesGenerate, bool saveUserInCache) {
            return GetNewOrExistingUserCache(UserManager.GetCurrentUser(), rolesGenerate, saveUserInCache);
        }

        private bool _isAdmin;
        private bool _isAdminRoleGenerated;
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
                } else {
                    _isAdmin = IsInRole(RoleNames.Admin);
                    _isAdminRoleGenerated = true;
                    return _isAdmin;
                }
            }
            set { _isAdmin = value; }
        }

        public bool IsAuth {
            get {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
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
        ///  Get user cache from session.
        /// </summary>
        /// <returns></returns>
        public static UserCache GetFromSession() {
            return HttpContext.Current.Session["UserCacheObject"] as UserCache;
        }
        /// <summary>
        /// Set usercache in session.
        /// </summary>
        /// <param name="userCache"></param>
        public static void SetInSession(UserCache userCache) {
            HttpContext.Current.Session["UserCacheObject"] = userCache;
        }
        /// <summary>
        /// Clear user cache session.
        /// </summary>
        public static void ClearSession() {
            HttpContext.Current.Session["UserCacheObject"] = null;
        }

    }
}