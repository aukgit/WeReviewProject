
namespace ReviewApps.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class CellPhone
    {
        public long CellPhoneID { get; set; }
        public long UserID { get; set; }
        public byte PlatformID { get; set; }
        public double PlatformVersion { get; set; }
    
        public virtual Platform Platform { get; set; }
        public virtual User User { get; set; }
    }
}
