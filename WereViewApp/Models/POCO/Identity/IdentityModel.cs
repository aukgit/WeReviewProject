using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WeReviewApp.Models.Context;

namespace WeReviewApp.Models.POCO.Identity {



    #region Login

    public class ApplicationUserLogin : IdentityUserLogin<long> {}

    #endregion

    #region Claim

    public class ApplicationUserClaim : IdentityUserClaim<long> {
        [DataType("VARCHAR")]
        [StringLength(20)]
        public override string ClaimType { get; set; }

        [DataType("VARCHAR")]
        [StringLength(80)]
        public override string ClaimValue { get; set; }
    }

    #endregion

    #region Application Roles
    public class ApplicationUserRole : IdentityUserRole<long> {

    }


    public class ApplicationRoleStore : RoleStore<ApplicationRole, long, ApplicationUserRole> {
        public ApplicationRoleStore(ApplicationDbContext context)
            : base(context) {
        }
    }

    public class ApplicationRoleManager : RoleManager<ApplicationRole, long> {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, long> roleStore)
            : base(roleStore) {
        }

        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context) {
            return new ApplicationRoleManager(
                new RoleStore<ApplicationRole, long, ApplicationUserRole>(context.Get<ApplicationDbContext>()));
        }
    }
    #endregion

    #region Application User Store
    public class ApplicatonUserStore :
       UserStore<ApplicationUser, ApplicationRole, long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim> {
        public ApplicatonUserStore(ApplicationDbContext context)
            : base(context) {
        }
    }

    #endregion


}