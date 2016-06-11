using WeReviewApp.Common;
using WeReviewApp.Models.EntityModel.Structs;
using WeReviewApp.Modules.Uploads;

namespace WeReviewApp.Models.EntityModel.ExtenededWithCustomMethods {
    public static class GalleryExtented {


        /// <summary>
        /// Returns image http url.
        /// </summary>
        /// <param name="gallery"></param>
        /// <param name="categoryId">
        /// If category given then returns based on category not in the gallery category.
        /// Useful while working with thumbs on gallery/ gallery icon.
        /// </param>
        /// <returns></returns>
        public static string GetHtppUrl(this Gallery gallery, int? categoryId = null) {
            var location = "";
            if (gallery != null && categoryId == null) {
                if (gallery.GalleryCategoryID == GalleryCategoryIDs.AppPageGallery) {
                    location = Statics.UProcessorGallery.GetCombinePathWithAdditionalRoots();
                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.GalleryIcon) {
                    location = Statics.UProcessorGalleryIcons.GetCombinePathWithAdditionalRoots();
                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.HomePageFeatured) {
                    location = Statics.UProcessorHomeFeatured.GetCombinePathWithAdditionalRoots();

                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.HomePageIcon) {
                    location = Statics.UProcessorHomeIcons.GetCombinePathWithAdditionalRoots();

                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.SearchIcon) {
                    location = Statics.UProcessorSearchIcons.GetCombinePathWithAdditionalRoots();

                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.SuggestionIcon) {
                    location = Statics.UProcessorSuggestionIcons.GetCombinePathWithAdditionalRoots();

                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.Advertise) {
                    location = Statics.UProcessorAdvertiseImages.GetCombinePathWithAdditionalRoots();

                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.YoutubeCoverImage) {
                    location = Statics.UProcessorYoutubeCover.GetCombinePathWithAdditionalRoots();
                }


            } else if (gallery != null && categoryId != null) {
                if (categoryId == GalleryCategoryIDs.AppPageGallery) {
                    location = Statics.UProcessorGallery.GetCombinePathWithAdditionalRoots();
                } else if (categoryId == GalleryCategoryIDs.GalleryIcon) {
                    location = Statics.UProcessorGalleryIcons.GetCombinePathWithAdditionalRoots();
                } else if (categoryId == GalleryCategoryIDs.HomePageFeatured) {
                    location = Statics.UProcessorHomeFeatured.GetCombinePathWithAdditionalRoots();

                } else if (categoryId == GalleryCategoryIDs.HomePageIcon) {
                    location = Statics.UProcessorHomeIcons.GetCombinePathWithAdditionalRoots();

                } else if (categoryId == GalleryCategoryIDs.SearchIcon) {
                    location = Statics.UProcessorSearchIcons.GetCombinePathWithAdditionalRoots();

                } else if (categoryId == GalleryCategoryIDs.SuggestionIcon) {
                    location = Statics.UProcessorSuggestionIcons.GetCombinePathWithAdditionalRoots();

                } else if (categoryId == GalleryCategoryIDs.Advertise) {
                    location = Statics.UProcessorAdvertiseImages.GetCombinePathWithAdditionalRoots();

                } else if (categoryId == GalleryCategoryIDs.YoutubeCoverImage) {
                    location = Statics.UProcessorYoutubeCover.GetCombinePathWithAdditionalRoots();
                }
            }

            if (gallery != null) {
                var fileName = UploadProcessor.GetOrganizeNameStatic(gallery, true);
                return AppVar.Url + location + fileName;
            }

            return null;
        }

    }
}