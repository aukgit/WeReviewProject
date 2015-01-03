using System.ComponentModel.DataAnnotations;

namespace WereViewApp.Models.POCO.IdentityCustomization {
    public class CountryDetectByIP {
        [Key]
        public int CountryDetectByIPID { get; set; }
        public int BeginingIP { get; set; }
        public int EndingIP { get; set; }

        public int CountryID { get; set; }


    }
}