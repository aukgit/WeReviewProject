using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WereViewApp.Models.POCO.IdentityCustomization {
    public class Feedback {
        public long FeedbackID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        [Display(Name = "Username",
            Description = "(ASCII)Only members should give their user name to speed up the process.")]
        public string Username { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Description = "(ASCII) Your authentic name.")]
        [StringLength(30)]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(
            Description = "(ASCII) Your feedback's subject essential to boost your solution. Limited to 150 character.")
        ]
        [Required]
        [StringLength(150)]
        [MinLength(30, ErrorMessage = "Please write a little bit more in your subject.")]
        public string Subject { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(
            Description = "(ASCII) Your feedback or comments or question about something. Limited to 800 character.")]
        [Required]
        [MinLength(70, ErrorMessage = "Please write a little bit more description.")]
        [StringLength(800)]
        public string Message { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Description = "(ASCII) Your email address.")]
        [StringLength(256)]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Urgent", Description = "Indicate your urgency.")]
        public float RateUrgency { get; set; }

        [Display(Description = "Response from administration")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(256)]
        public string Response { get; set; }

        public bool IsViewed { get; set; }
        public bool IsInProcess { get; set; }
        public bool IsSolved { get; set; }
        public bool IsUnSolved { get; set; }

        [Display(Name = "Has follow up date.", Description = "Has follow up date.")]
        public bool HasMarkedToFollowUpDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime PostedDate { get; set; }

        public DateTime? FollowUpdateDate { get; set; }

        /// <summary>
        ///     Represents a possibility of app or review has been reported by a user
        /// </summary>
        public bool HasAppOrReviewReport { get; set; }

        [Display(Name = "Category",
            Description =
                "Please select a category from the list, in case if you want to report app , please visit the app and click on report."
            )]
        public byte FeedbackCategoryID { get; set; }

        [ForeignKey("FeedbackID")]
        public ICollection<FeedbackAppReviewRelation> FeedbackAppReviewRelations { get; set; }
    }
}