using System;
using WeReviewApp.Models.EntityModel;

namespace WeReviewApp.Models.CustomQuery {
    public class CommonQueriesWereViewApp :IDisposable {
        private readonly WereViewAppEntities db = new WereViewAppEntities();

        public GalleryCategory GetGallery(Gallery gallery) {
            return db.GalleryCategories.Find(gallery.GalleryCategoryID);
        }
        public GalleryCategory GetGallery(int id) {
            return db.GalleryCategories.Find(id);
        }

        public void Dispose() {
            db.Dispose();
        }
    }
}