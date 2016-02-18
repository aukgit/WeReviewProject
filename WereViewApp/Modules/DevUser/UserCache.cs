using System;
using System.Collections.Generic;
using System.Web;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Modules.Role;
using System.Linq;
namespace WereViewApp.Modules.DevUser {
    public class UserCache {
        /// <summary>
        /// On creation get all the user roles to the cache.
        /// </summary>
        public UserCache() {
            GenerateRoles();
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
                role = Roles.FirstOrDefault(n => n.Name.Equals(roleName, System.StringComparison.OrdinalIgnoreCase));
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
                this.User = UserManager.GetCurrentUser();
                Roles = RoleManager.GetUserRolesAsApplicationRole(this.Username);
            } else {
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