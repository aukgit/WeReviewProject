using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WereViewApp.Models.POCO.Identity;
using System.ComponentModel.DataAnnotations;

namespace WereViewApp.Models.ViewModels {
    public class ManageRolesViewModel {
        [Key]
        public long UserId { get; set; }
        public List<ApplicationRole> AllRoles { get; set; }
        public List<ApplicationRole> UserInRoles { get; set; }
        [Display(Name = "Name")]
        public string UserDisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role">A role from AllRoles will be matched with UserInRoles</param>
        /// <returns></returns>
        public bool IsCurrentRoleRelatedToUser(ApplicationRole role) {
            return UserInRoles.Any(n => n.Id == role.Id);
        }
    }
}