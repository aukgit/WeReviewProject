namespace WeReviewApp.Models.EntityModel {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Review {
        public Review() {
            this.ReviewLikeDislikes = new HashSet<ReviewLikeDislike>();
        }

        public long ReviewID { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Description = "(ASCII 30) Title of your review. What you think in short.")]
        public string Title { get; set; }
        //[Required]
        [StringLength(300)]
        //[MinLength(50, ErrorMessage = "It should be minimum 50 characters with valid points.")]
        [Display(Name = "Pros", Description = "(ASCII 300) Pros about this app.")]
        [DisplayName("Pros")]
        public string Pros { get; set; }
        
        [StringLength(300)]
        [MinLength(50, ErrorMessage = "It should be minimum 50 characters with valid points.")]
        [Display(Description = "(ASCII 300) Cons about this app.")]
        [DisplayName("Cons")]
        public string Cons { get; set; }
        [Required]
        [Display(Name = "Recommend other users.")]
        public bool IsSuggest { get; set; }
        [StringLength(100)]
        public string Comment1 { get; set; }
        [StringLength(500)]
        public string Comment2 { get; set; }
        [Required]
        public long AppID { get; set; }
        [Required]
        public long UserID { get; set; }

        [StringLength(590)]
        [Required]
        [Display(Name = "Comments", Description = "(ASCII 590) Thoughts/Summary about this app.")]
        [MinLength(10, ErrorMessage = "It should be minimum 60 characters with valid points.")]
        //[DisplayName("Thoughts")]
        public string Comments { get; set; }

        [DisplayName("Liked")]
        public int LikedCount { get; set; }
        [DisplayName("Disliked")]
        public int DisLikeCount { get; set; }
        [Required]
        public byte Rating { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual App App { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ReviewLikeDislike> ReviewLikeDislikes { get; set; }
    }
}
