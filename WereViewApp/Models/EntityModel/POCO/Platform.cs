namespace WereViewApp.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Platform
    {
        public Platform()
        {
            this.Apps = new HashSet<App>();
            this.CellPhones = new HashSet<CellPhone>();
        }
    
        public byte PlatformID { get; set; }
        [Display(Name="Platform Name", Description="Like Apple, Android...")]
        public string PlatformName { get; set; }
        public string Icon { get; set; }
    
        public virtual ICollection<App> Apps { get; set; }
        public virtual ICollection<CellPhone> CellPhones { get; set; }
    }
}
