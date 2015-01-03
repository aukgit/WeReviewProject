using WereViewApp.Models.DesignPattern.Interfaces;
using WereViewApp.Models.POCO.IdentityCustomization;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WereViewApp.Models.POCO.Identity {
    public class ApplicationRole : IdentityRole<long, ApplicationUserRole>, IDevUserRole {
        //[Display(Name="Can be achieive by points.")]
        //public bool CanAchieveByPoints { get; set; }
        //[Display(Name = "Point Required")]
        //public float PointRequired { get; set; }

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