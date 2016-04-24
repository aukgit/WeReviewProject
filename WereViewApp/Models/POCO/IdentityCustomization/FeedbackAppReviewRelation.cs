using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeReviewApp.Models.POCO.IdentityCustomization {
    public class FeedbackAppReviewRelation {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FeedbackAppReviewRelationID { get; set; }
        public long FeedbackID { get; set; }
        /// <summary>
        /// Returns -1 if not valid
        /// </summary>
        public long AppID { get; set; }
        /// <summary>
        /// Returns -1 if not valid
        /// </summary>
        public long ReviewID { get; set; }
        /// <summary>
        /// True :Represents if contains app id
        /// False : Represents ReviewId valid
        /// </summary>
        public bool HasAppId { get; set; }

    }
}