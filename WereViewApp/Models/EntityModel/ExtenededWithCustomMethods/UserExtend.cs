
using WeReviewApp.Models.POCO.Identity;

namespace WeReviewApp.Models.EntityModel.ExtenededWithCustomMethods {
    public static class UserExtented {
        /// <summary>
        /// Gets current user's profile url.
        /// </summary>
        /// <returns>Returns current user's profile url.</returns>
        public static string GetProfileUrl(this ApplicationUser user) {
            return AppVar.Url + "/profiles/" + user.UserName;
        }


        /// <summary>
        /// Gets current user's profile url.
        /// </summary>
        /// <returns>Returns current user's profile url.</returns>
        public static string GetProfileUrl(this User user) {
            return AppVar.Url + "/profiles/" + user.UserName;
        }
    }
}