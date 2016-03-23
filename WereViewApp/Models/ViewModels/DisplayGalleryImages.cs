using System;

namespace WeReviewApp.Models.ViewModels {
    public class DisplayGalleryImages {
        public Guid GalleryID { get; set; }

        public int Sequence { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string GalleryImageLocation { get; set; }

        public string GalleryIconLocation { get; set; }
    }
}