using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.POCO;

namespace WereViewApp.Models.CustomQuery {
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