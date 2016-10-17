using System;
using ReviewApps.Models.EntityModel;

namespace ReviewApps.Models.CustomQuery {
    public class CommonQueriesWereViewApp : IDisposable {
        private readonly ReviewAppsEntities db = new ReviewAppsEntities();

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