//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WereViewApp.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class FeaturedImage
    {
        public long FeaturedImageID { get; set; }
        public long AppID { get; set; }
        public bool IsFeatured { get; set; }
        public long UserID { get; set; }
    
        public virtual App App { get; set; }
        public virtual User User { get; set; }
    }
}