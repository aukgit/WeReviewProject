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
    
    public partial class NotificationType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NotificationType()
        {
            this.Notifications = new HashSet<Notification>();
            this.NotificationSeens = new HashSet<NotificationSeen>();
        }
    
        public byte NotificationTypeID { get; set; }
        public string TypeName { get; set; }
        public bool IsGood { get; set; }
        public string DefaultMessage { get; set; }
        public string MessageIconName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotificationSeen> NotificationSeens { get; set; }
    }
}
