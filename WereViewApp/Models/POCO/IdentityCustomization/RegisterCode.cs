using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeReviewApp.Models.POCO.IdentityCustomization {
    public class RegisterCode {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RegisterCodeID { get; set; }
        public long RoleID { get; set; }

        public DateTime GeneratedDate { get; set; }

        public DateTime ValidityTill { get; set; }

        public bool IsUsed { get; set; }
        public bool IsExpired { get; set; }
    }
}