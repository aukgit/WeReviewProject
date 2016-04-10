using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeReviewApp.Models.POCO.IdentityCustomization {
    public class FeedbackCategory {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte FeedbackCategoryID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Required]
        [StringLength(30)]
        public string Category { get; set; }

        [ForeignKey("FeedbackCategoryID")]
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}