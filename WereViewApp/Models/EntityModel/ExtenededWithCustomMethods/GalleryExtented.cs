using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Modules.Uploads;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Models.EntityModel.ExtenededWithCustomMethods {
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
            string location = "";
            if (gallery != null && categoryId == null) {
                if (gallery.GalleryCategoryID == GalleryCategoryIDs.AppPageGallery) {
                    location = WereViewStatics.UProcessorGallery.GetCombinationOfRootAndAdditionalRoot();
                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.GalleryIcon) {
                    location = WereViewStatics.UProcessorGalleryIcons.GetCombinationOfRootAndAdditionalRoot();
                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.HomePageFeatured) {
                    location = WereViewStatics.UProcessorHomeFeatured.GetCombinationOfRootAndAdditionalRoot();

                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.HomePageIcon) {
                    location = WereViewStatics.UProcessorHomeIcons.GetCombinationOfRootAndAdditionalRoot();

                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.SearchIcon) {
                    location = WereViewStatics.UProcessorSearchIcons.GetCombinationOfRootAndAdditionalRoot();

                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.SuggestionIcon) {
                    location = WereViewStatics.UProcessorSuggestionIcons.GetCombinationOfRootAndAdditionalRoot();

                } else if (gallery.GalleryCategoryID == GalleryCategoryIDs.Advertise) {
                    location = WereViewStatics.UProcessorAdvertiseImages.GetCombinationOfRootAndAdditionalRoot();

                }


            } else if (gallery != null && categoryId != null) {
                if (categoryId == GalleryCategoryIDs.AppPageGallery) {
                    location = WereViewStatics.UProcessorGallery.GetCombinationOfRootAndAdditionalRoot();
                } else if (categoryId == GalleryCategoryIDs.GalleryIcon) {
                    location = WereViewStatics.UProcessorGalleryIcons.GetCombinationOfRootAndAdditionalRoot();
                } else if (categoryId == GalleryCategoryIDs.HomePageFeatured) {
                    location = WereViewStatics.UProcessorHomeFeatured.GetCombinationOfRootAndAdditionalRoot();

                } else if (categoryId == GalleryCategoryIDs.HomePageIcon) {
                    location = WereViewStatics.UProcessorHomeIcons.GetCombinationOfRootAndAdditionalRoot();

                } else if (categoryId == GalleryCategoryIDs.SearchIcon) {
                    location = WereViewStatics.UProcessorSearchIcons.GetCombinationOfRootAndAdditionalRoot();

                } else if (categoryId == GalleryCategoryIDs.SuggestionIcon) {
                    location = WereViewStatics.UProcessorSuggestionIcons.GetCombinationOfRootAndAdditionalRoot();

                } else if (categoryId == GalleryCategoryIDs.Advertise) {
                    location = WereViewStatics.UProcessorAdvertiseImages.GetCombinationOfRootAndAdditionalRoot();

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