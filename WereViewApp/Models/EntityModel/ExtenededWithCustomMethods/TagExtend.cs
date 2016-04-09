

namespace WeReviewApp.Models.EntityModel.ExtenededWithCustomMethods {
    public static class TagExtented {
        /// <summary>
        /// Gets current user's profile url.
        /// </summary>
        /// <returns>Returns current user's profile url.</returns>
        public static string GetTagUrl(this Tag tag) {
            return AppVar.Url + "/tags/" + tag.TagDisplay;
        }
   
    }
}