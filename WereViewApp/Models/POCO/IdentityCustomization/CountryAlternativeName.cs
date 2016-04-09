using System.ComponentModel.DataAnnotations;

namespace WeReviewApp.Models.POCO.IdentityCustomization {
    public class CountryAlternativeName {
        [Key]
        public int CountryAlternativeNameID { get; set; }
        [StringLength(80)]
        [Required]
        public string AlternativeName { get; set; }

        public int CountryID { get; set; }

    }
}
