using System;

namespace WeReviewApp.Models.ViewModels {
    public class UploadedGalleryImageEditViewModel {
        public long Id { get; set; }
        public string ImageURL { get; set; }
        public string DeleteURL { get; set; }
        public string ReUploadURL { get; set; }

        //[RegularExpression(@"^([0aA -Zz9])[^\_\-\=\!\@\$\%\^\&\*\(\)]+")]
        public string Tile { get; set; }
        //[RegularExpression(@"^([0aA -Zz9])[^\_\-\=\!\@\$\%\^\&\*\(\)]+")]
        public string Subtitle { get; set; }

        public Guid UploadGuid { get; set; }
        public byte Sequence { get; set; }
    }
}