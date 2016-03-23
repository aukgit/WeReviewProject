//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeReviewApp.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Subscribe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Subscribe()
        {
            this.SubscribeNotifiedHistories = new HashSet<SubscribeNotifiedHistory>();
        }
    
        public long SubscribeID { get; set; }
        public string Email { get; set; }
        public bool IsSubscriberMemeber { get; set; }
        public short SubscribeCategoryID { get; set; }
        public Nullable<long> SubcribeToUserID { get; set; }
        public Nullable<System.DateTime> LastNotifiedOnDate { get; set; }
        public Nullable<long> SubscribedByUserID { get; set; }
    
        public virtual SubscribeCategory SubscribeCategory { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubscribeNotifiedHistory> SubscribeNotifiedHistories { get; set; }
    }
}
