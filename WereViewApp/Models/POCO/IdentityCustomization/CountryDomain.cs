using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeReviewApp.Models.POCO.IdentityCustomization {
    public class CountryDomain {
        public int CountryDomainID { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(6)]
        public string Domain { get; set; }

        public int CountryID { get; set; }
    }
}
