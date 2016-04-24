using System.ComponentModel.DataAnnotations;

namespace WeReviewApp.Models.POCO.IdentityCustomization {
    public class CountryDetectByIP {
        [Key]
        public int CountryDetectByIPID { get; set; }
        public long BeginingIP { get; set; }
        public long EndingIP { get; set; }

        public int CountryID { get; set; }


    }
}