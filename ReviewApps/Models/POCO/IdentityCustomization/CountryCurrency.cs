using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApps.Models.POCO.IdentityCustomization {
    public class CountryCurrency {
        [Key]
        public int CountryCurrencyID { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(7)]
        [Required]
        public string CurrencyName { get; set; }

        public int CountryID { get; set; }

    }
}
