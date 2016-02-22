using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DevMvcComponent;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Models.POCO.IdentityCustomization;
using WereViewApp.Modules.DevUser;

namespace WereViewApp.Modules.Role {
    /// <summary>
    ///     Developers Organism Role Manager
    /// </summary>
    public class RoleManager {
        private static RoleStore<ApplicationRole, long, ApplicationUserRole> _roleStore =
            new RoleStore<ApplicationRole, long, ApplicationUserRole>(new ApplicationDbContext());

        private static ApplicationRoleManager _roleManager;
        private static List<ApplicationRole> _cachesOfUsers = new List<ApplicationRole>(1200);


        public static ApplicationRoleManager Manager
        {
            get
            {
                if (_roleManager == null) {
                    _roleManager = new ApplicationRoleManager(_roleStore);
                }
                return _roleManager;
            }
        }

        #region Manager Reset

        public static ApplicationRoleManager ResetManager() {
            _roleStore = new RoleStore<ApplicationRole, long, ApplicationUserRole>(new ApplicationDbContext());
            _roleManager = new ApplicationRoleManager(_roleStore);
            UserCache.ClearSession();
            GC.Collect();
            return _roleManager;
        }

        #endregion

        #region Get Users Highest Role

        /// <summary>
        ///     Get the high role. Which has low priority in the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static ApplicationRole GetHighestRole(long userId) {
            if (UserManager.IsAuthenticated()) {
                var roles = GetUserRolesAsApplicationRole(userId);

                var priorityLow = roles.Min(n => n.PriorityLevel);
                var highestPriorityRole = roles.FirstOrDefault(n => n.PriorityLevel == priorityLow);
                return highestPriorityRole;
            }
            return null;
        }

        #endregion

        #region Get roles Id

        /// <summary>
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>Returns -1 if not exist.</returns>
        public static long GetRoleId(string roleName) {
            if (!string.IsNullOrEmpty(roleName)) {
                var role = GetRole(roleName);
                if (role != null) {
                    return role.Id;
                }
            }
            return -1;
        }

        #endregion

        #region Role Existence check

        /// <summary>
        ///     if role exists.
        /// </summary>
        /// <param name="roleName"></param>
        public static bool Exists(string roleName) {
            return Manager.RoleExists(roleName);
        }

        #endregion

        #region Get and remove temporary role information

        /// <summary>
        ///     Returns temporary role and remove the infor from temp.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<ApplicationRole> ReturnRoleIdFromTempInfoAndRemoveTemp(long userId) {
            using (var db = new ApplicationDbContext()) {
                var temp = db.TempUserRoleRelations.FirstOrDefault(n => n.UserID == userId);
                if (temp != null) {
                    var role = GetRole(temp.UserRoleID);
                    if (role != null) {
                        db.TempUserRoleRelations.Remove(temp);
                        await db.SaveChangesAsync();
                        return role;
                    }
                    throw new Exception(
                        "Saved information in temp at the time of registration doesn't match any role in database.");
                }
            }
            return null;
        }

        #endregion

        #region Create

        /// <summary>
        ///     First check if role exist or not.
        ///     It will not work if new properties are added to the role table.
        ///     Use overload if new properties are added.
        /// </summary>
        /// <param name="roleName"></param>
        public static void CreateRole(string roleName) {
            var exist = Manager.RoleExists(roleName);
            if (!exist) {
                var role = new ApplicationRole {Name = roleName};
                try {
                    Manager.Create(role);
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
        }

        /// <summary>
        ///     First check if role exist or not.
        /// </summary>
        /// <param name="roleName"></param>
        public static void CreateRole(ApplicationRole role) {
            var exist = Manager.RoleExists(role.Name);
            if (!exist) {
                try {
                    Manager.Create(role);
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
        }

        #endregion

        #region Get Roles or Single Role

        /// <summary>
        ///     Get all the roles.
        /// </summary>
        /// <param name="roleName"></param>
        public static List<ApplicationRole> GetRoles() {
            using (var db = new ApplicationDbContext()) {
                return db.Roles.ToList();
            }
        }

        /// <summary>
        ///     Get all the roles by search naming the role name with wildcard.
        /// </summary>
        /// <param name="roleName"></param>
        public static List<ApplicationRole> GetRoles(string search) {
            return Manager.Roles.Where(n => n.Name.Contains(search)).ToList();
        }


        public static ApplicationRole GetRole(string roleName) {
            return Manager.Roles.FirstOrDefault(n => n.Name == roleName);
        }

        public static ApplicationRole GetRole(long id) {
            return Manager.Roles.FirstOrDefault(n => n.Id == id);
        }

        #endregion

        #region Is In Role/ Has Role

        /// <summary>
        ///     Check if user is in this role.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static bool IsInRole(long userId, string role) {
            return UserManager.Manager.IsInRole(userId, role);
        }

        /// <summary>
        ///     Is current user in checking role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static bool IsInRole(string role) {
            if (UserManager.IsAuthenticated()) {
                return UserManager.Manager.IsInRole(UserManager.GetCurrentUser().UserID, role);
            }
            return false;
        }

        public static bool HasMiniumRole(long userId, string roleName) {
            var role = GetRole(roleName);
            if (role != null) {
                using (var db = new ApplicationDbContext()) {
                    var anyAboveRoles = db.Roles.Where(r => r.PriorityLevel <= role.PriorityLevel);
                    return anyAboveRoles.Any(n => IsInRole(userId, n.Name));
                }
            }
            return false;
        }

        public static bool HasMiniumRole(string roleName) {
            if (UserManager.IsAuthenticated()) {
                var userId = UserManager.GetCurrentUser().UserID;
                return HasMiniumRole(userId, roleName);
            }
            return false;
        }

        #endregion

        #region Get Users Roles

        /// <summary>
        ///     Get roles based on user id (Faster);
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IList<string> GetUserRoles(long userId) {
            return UserManager.Manager.GetRoles(userId);
        }

        /// <summary>
        ///     Get roles based on user id (Faster);
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List or null.</returns>
        public static IList<string> GetUserRoles(string username) {
            var user = UserManager.GetUser(username);
            if (user != null) {
                return UserManager.Manager.GetRoles(user.UserID);
            }
            return null;
        }


        /// <summary>
        ///     Give all related roles to this user.
        ///     Slower
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<ApplicationRole> GetUserRolesAsApplicationRole(string username) {
            var user = UserManager.GetUser(username);
            if (user != null) {
                return GetUserRolesAsApplicationRole(user.UserID);
            }
            return null;
        }

        /// <summary>
        ///     Give all related roles to this user.
        ///     Faster
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<ApplicationRole> GetUserRolesAsApplicationRole(long userId) {
            using (var db2 = new ApplicationDbContext()) {
                return db2.Roles.Where(n => n.Users.Any(u => u.UserId == userId)).ToList();
            }
        }

        #endregion

        #region Get Underlying roles based on priority

        /// <summary>
        ///     Get current one and all underlying roles from this role
        ///     in custom db.
        ///     Condition :  Return all above priority value roles which represents the underlying roles.
        /// </summary>
        /// <param name="roleName">Name of your current role will return all underlying priority roles</param>
        /// <returns></returns>
        private static List<ApplicationRole> GetUnderlyingRoles(string roleName) {
            var role = GetRole(roleName);
            if (role != null) {
                using (var db = new ApplicationDbContext()) {
                    var roles = db.Roles.Where(n => n.PriorityLevel >= role.PriorityLevel).ToList();
                    return roles;
                }
            }
            return null;
        }

        /// <summary>
        ///     Get all underlying roles from this role
        ///     in custom db.
        /// </summary>
        /// <param name="role">return all underlying priority roles</param>
        /// <returns></returns>
        private static List<ApplicationRole> GetUnderlyingRoles(ApplicationRole role) {
            if (role != null) {
                using (var db = new ApplicationDbContext()) {
                    var roles = db.Roles.Where(n => n.PriorityLevel > role.PriorityLevel).ToList();
                    return roles;
                }
            }
            return null;
        }

        #endregion

        #region Add roles by priority : add all underlying roles

        /// <summary>
        ///     Add all underlying roles
        ///     Verifies user before adding roles.
        /// </summary>
        /// <param name="log">Username</param>
        /// <param name="role">Current role will add all underlying roles</param>
        public static void AddUnderlyingRoles(string log, string role) {
            if (string.IsNullOrWhiteSpace(log) || string.IsNullOrWhiteSpace(role)) {
                return;
            }
            var user = UserManager.GetUser(log);
            if (user != null) {
                var roles = GetUnderlyingRoles(role);
                if (roles != null) {
                    foreach (var sRole in roles) {
                        AddRoleToUser(user, sRole.Name);
                    }
                }
            } else {
                throw new Exception("User doesn't exist.");
            }
        }

        /// <summary>
        ///     Add all underlying roles based on (
        ///     Verifies user before adding roles.
        ///     (Faster)
        /// </summary>
        /// <param name="log">Username</param>
        /// <param name="role">Current role will add all underlying roles</param>
        public static void AddUnderlyingRoles(ApplicationUser user, string role) {
            if (user != null) {
                var roles = GetUnderlyingRoles(role);
                if (roles != null) {
                    foreach (var sRole in roles) {
                        AddRoleToUser(user, sRole.Name);
                    }
                }
            } else {
                throw new Exception("User doesn't exist.");
            }
        }

        #endregion

        #region Remove by priority : remove all underlying roles

        /// <summary>
        ///     Add all underlying roles
        ///     Verifies user before adding roles.
        /// </summary>
        /// <param name="log">Username</param>
        /// <param name="role">Current role will add all underlying roles</param>
        public static void RemoveUnderlyingRoles(string log, string role) {
            if (string.IsNullOrWhiteSpace(log) || string.IsNullOrWhiteSpace(role)) {
                return;
            }
            var user = UserManager.GetUser(log);
            if (user != null) {
                var roles = GetUnderlyingRoles(role);
                if (roles != null) {
                    foreach (var sRole in roles) {
                        RemoveUserRole(user, sRole.Name);
                    }
                }
            } else {
                throw new Exception("User doesn't exist.");
            }
        }

        /// <summary>
        ///     Faster
        ///     Add all underlying roles
        ///     Verifies user before adding roles.
        /// </summary>
        /// <param name="log">Username</param>
        /// <param name="role">Current role will add all underlying roles</param>
        public static void RemoveUnderlyingRoles(ApplicationUser user, string role) {
            if (user != null) {
                var roles = GetUnderlyingRoles(role);
                if (roles != null) {
                    foreach (var sRole in roles) {
                        RemoveUserRole(user, sRole.Name);
                    }
                }
            } else {
                throw new Exception("User doesn't exist.");
            }
        }

        #endregion

        #region Get users in role

        /// <summary>
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>Returns null if not exist.</returns>
        public static List<ApplicationUser> GetUsersInRole(string roleName) {
            var roleId = GetRoleId(roleName);
            if (roleId != -1) {
                return GetUsersInRole(roleId);
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>Returns null if not exist.</returns>
        public static List<ApplicationUser> GetUsersInRole(long roleId) {
            using (var db = new ApplicationDbContext()) {
                return db.Users.Where(e => !e.Roles.All(r => r.RoleId == roleId)).ToList();
            }
        }


        /// <summary>
        ///     Check if user in role by user name.
        /// </summary>
        /// <param name="roleName"></param>
        public static bool UserInRole(string username, string role) {
            var user = UserManager.GetUser(username);
            if (user != null) {
                return UserManager.Manager.IsInRole(user.Id, role);
            }
            return false;
        }

        #endregion

        #region Is user exist inside a role

        /// <summary>
        ///     if any user exist with this given role
        /// </summary>
        /// <param name="role"></param>
        /// <returns>Returns false if role doesn't exist or not found any users.</returns>
        public static bool IsAnyRelatedUsers(string role) {
            var roleId = GetRoleId(role);
            if (roleId != -1) {
                return IsAnyRelatedUsers(roleId);
            }
            return false;
        }

        /// <summary>
        ///     if any user exist with this given role
        ///     (Faster)
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static bool IsAnyRelatedUsers(long roleId) {
            using (var db = new ApplicationDbContext()) {
                return db.Users.Any(e => e.Roles.All(r => r.RoleId == roleId));
            }
        }

        #endregion

        #region Remove user roles

        /// <summary>
        ///     Remove role from the user.
        ///     Faster
        /// </summary>
        /// <param name="role"></param>
        public static void RemoveRole(ApplicationRole role) {
            Manager.Delete(role);
        }

        /// <summary>
        ///     Remove role from the user.
        /// </summary>
        /// <param name="roleName"></param>
        public static void RemoveRole(string roleName) {
            var role = GetRole(roleName);
            if (role != null) {
                Manager.Delete(role);
            }
        }

        /// <summary>
        ///     Remove role from the user.
        /// </summary>
        /// <param name="roleId"></param>
        public static void RemoveRole(long roleId) {
            var role = GetRole(roleId);
            if (role != null) {
                Manager.Delete(role);
            }
        }

        #endregion

        #region User belongs to a role

        /// <summary>
        ///     Check if user in role by user name.
        /// </summary>
        /// <param name="roleName"></param>
        public static bool UserInRole(ApplicationUser user, string role) {
            if (user != null) {
                return UserManager.Manager.IsInRole(user.Id, role);
            }
            return false;
        }

        /// <summary>
        ///     Check if current user in role.
        /// </summary>
        /// <param name="roleName"></param>
        public static bool UserInRole(string role) {
            return HttpContext.Current.User.IsInRole(role);
        }

        #endregion

        #region Add roles to user

        /// <summary>
        ///     Add current user to the role.
        /// </summary>
        /// <param name="roleName"></param>
        public static bool AddRoleToUser(string role) {
            var user = UserManager.GetCurrentUser();
            if (user != null) {
                try {
                    UserManager.Manager.AddToRole(user.Id, role);
                    return true;
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
            return false;
        }

        /// <summary>
        ///     Add user to the role.
        /// </summary>
        /// <param name="roleName"></param>
        public static bool AddRoleToUser(string username, string role) {
            var user = UserManager.GetUser(username);
            if (user != null) {
                try {
                    UserManager.Manager.AddToRole(user.Id, role);
                    return true;
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
            return false;
        }


        /// <summary>
        ///     Faster
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static bool AddRoleToUser(long userId, long roleId) {
            var role = GetRole(roleId);
            if (role != null) {
                AddRoleToUser(userId, role.Name);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     More Faster
        /// </summary>
        public static void AddRoleToUser(long userId, string roleName) {
            UserManager.Manager.AddToRole(userId, roleName);
        }

        /// <summary>
        ///     Add user to the role.
        /// </summary>
        /// <param name="roleName"></param>
        public static bool AddRoleToUser(ApplicationUser user, string role) {
            if (user != null) {
                try {
                    UserManager.Manager.AddToRole(user.Id, role);
                    return true;
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
            return false;
        }

        /// <summary>
        ///     All roles will be added to all those users
        /// </summary>
        /// <param name="roleName"></param>
        public static void AddRolesToUser(string username, string[] roles) {
            foreach (var role in roles) {
                AddRoleToUser(username, role);
            }
        }

        /// <summary>
        ///     All roles will be added to all those users
        /// </summary>
        /// <param name="roleName"></param>
        public static void AddRoleToUsers(string[] usernames, string role) {
            foreach (var user in usernames) {
                AddRoleToUser(user, role);
            }
        }

        /// <summary>
        ///     All roles will be added to all those users
        /// </summary>
        /// <param name="roleName"></param>
        public static void AddRolesToUsers(string[] usernames, string[] roles) {
            foreach (var user in usernames) {
                foreach (var role in roles) {
                    AddRoleToUser(user, role);
                }
            }
        }

        #endregion

        #region Remove Roles

        /// <summary>
        ///     Remove role from user
        /// </summary>
        /// <param name="roleName"></param>
        public static bool RemoveUserRole(string username, string role) {
            var user = UserManager.GetUser(username);
            if (user != null) {
                try {
                    UserManager.Manager.RemoveFromRole(user.Id, role);
                    return true;
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
            return false;
        }

        /// <summary>
        ///     Faster
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static bool RemoveUserRole(long userId, long roleId) {
            var role = GetRole(roleId);
            if (role != null) {
                try {
                    UserManager.Manager.RemoveFromRole(userId, role.Name);
                    return true;
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
            return false;
        }

        /// <summary>
        ///     More faster
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        public static void RemoveUserRole(long userId, string roleName) {
            try {
                UserManager.Manager.RemoveFromRole(userId, roleName);
            } catch (Exception ex) {
                Mvc.Error.HandleBy(ex);
            }
        }

        /// <summary>
        ///     Remove role from user
        /// </summary>
        /// <param name="roleName"></param>
        public static bool RemoveUserRole(ApplicationUser user, string role) {
            if (user != null) {
                try {
                    UserManager.Manager.RemoveFromRole(user.Id, role);
                    return true;
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
            return false;
        }

        /// <summary>
        ///     Remove role from current user
        /// </summary>
        /// <param name="roleName"></param>
        public static bool RemoveUserRole(string role) {
            var user = UserManager.GetCurrentUser();
            if (user != null) {
                try {
                    UserManager.Manager.RemoveFromRole(user.Id, role);
                    return true;
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
            return false;
        }

        #endregion

        #region Get all roles of an user.

        /// <summary>
        ///     Get Roles by User name
        /// </summary>
        /// <param name="roleName"></param>
        public static List<string> GetAllAssignedRoles(string username) {
            var user = UserManager.GetUser(username);
            if (user != null) {
                return UserManager.Manager.GetRoles(user.Id).ToList();
            }
            return null;
        }

        /// <summary>
        ///     Get Roles by User
        /// </summary>
        /// <param name="roleName"></param>
        public static List<string> GetAllAssignedRoles(ApplicationUser user) {
            if (user != null) {
                return UserManager.Manager.GetRoles(user.Id).ToList();
            }
            return null;
        }

        /// <summary>
        ///     Get all assigned roles by current user
        /// </summary>
        /// <param name="roleName"></param>
        public static List<string> GetAllAssignedRoles() {
            var user = UserManager.GetCurrentUser();
            if (user != null) {
                return UserManager.Manager.GetRoles(user.Id).ToList();
            }
            return null;
        }

        #endregion

        #region Keep temp role information

        /// <summary>
        ///     Faster
        ///     Add a temporary information from the registration process
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleId">Roles doesn't verify inside the method</param>
        public static void AddTempRoleInfo(ApplicationUser user, long roleId) {
            using (var db = new ApplicationDbContext()) {
                var temp = new TempUserRoleRelation();
                temp.UserID = user.UserID;
                temp.UserRoleID = roleId;
                db.TempUserRoleRelations.Add(temp);
                db.SaveChanges();
            }
        }

        /// <summary>
        ///     Add a temporary information from the registration process
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleId">Role doesn't verify inside the method</param>
        public static void AddTempRoleInfo(long userId, long roleId) {
            var user = UserManager.GetUser(userId);
            if (user != null) {
                AddTempRoleInfo(user, roleId);
            }
        }

        #endregion
    }
}