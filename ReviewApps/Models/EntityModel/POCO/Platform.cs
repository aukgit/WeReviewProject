using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReviewApps.Models.EntityModel
{
    public class Platform
    {
        public Platform()
        {
            Apps = new HashSet<App>();
            CellPhones = new HashSet<CellPhone>();
        }
    
        public byte PlatformID { get; set; }
        [Display(Name="Platform Name", Description="Like Apple, Android...")]
        public string PlatformName { get; set; }
        public string Icon { get; set; }
    
        public virtual ICollection<App> Apps { get; set; }
        public virtual ICollection<CellPhone> CellPhones { get; set; }
    }
}
