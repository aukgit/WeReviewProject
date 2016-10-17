using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using ReviewApps.Models.DesignPattern.Interfaces;
using ReviewApps.Models.POCO.IdentityCustomization;

namespace ReviewApps.Models.POCO.Identity {
    public class ApplicationRole : IdentityRole<long, ApplicationUserRole>, IDevUserRole {
        //[Display(Name="Can be achieive by points.")]
        //public bool CanAchieveByPoints { get; set; }
        //[Display(Name = "Point Required")]
        //public float PointRequired { get; set; }

        /// <summary>
        ///     Lower priority means more power.
        /// </summary>
        [Display(Name = "Priority Level", Description = "Less is more.")]
        public byte PriorityLevel { get; set; }

        [Display(Name = "Can be achieved by points")]
        public bool CanBeAcheivedByPoint { get; set; }

        [Display(Name = "Point Required")]
        public long PointsRequired { get; set; }

        [ForeignKey("RoleID")]
        public virtual ICollection<RegisterCode> RegisterCodes { get; set; }

    }
}