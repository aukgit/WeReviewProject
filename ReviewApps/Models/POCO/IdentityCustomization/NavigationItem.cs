using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApps.Models.POCO.IdentityCustomization {
    public class NavigationItem {
        public int NavigationItemID { get; set; }
        public int NavigationID { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(250)]
        [Required]
        public string Title { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(250)]
        [Display(Name = "Element Classes", Description = "Html classes related to this list-item.")]
        public string ElementClasses { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        [Display(Name = "Element ID", Description = "Html ID related to this list-item. Prefer not to use because classes are the mordern practice.")]
        public string ElementID { get; set; }

        [Required]
        [StringLength(600)]
        public string RelativeURL { get; set; }
        public int Ordering { get; set; }
        public bool HasDropDown { get; set; }

        public int? ParentNavigationID { get; set; }



    }
}