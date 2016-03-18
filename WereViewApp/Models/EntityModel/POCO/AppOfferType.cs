namespace WereViewApp.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class AppOfferType
    {
        
        public AppOfferType()
        {
            this.Apps = new HashSet<App>();
        }
    
        public byte AppOfferTypeID { get; set; }
        public string OfferType { get; set; }
    
        
        public virtual ICollection<App> Apps { get; set; }
    }
}
