using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WereViewApp.Models.POCO.IdentityCustomization {
    public class ImageResizeSetting {
        public int ImageResizeSettingID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(256)]
        [Required]
        public string Name { get; set; }

        [Required]
        public float Height { get; set; }

        [Required]
        public float Width { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(5)]
        [Required]
        public string Extension { get; set; }
    }
}