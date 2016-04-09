using System.Web;
using WeReviewApp.Modules.Type;
using WeReviewApp.Modules.Extensions;

namespace WeReviewApp.Modules.Extensions {
    public static class HttpContextExtension {

        public static ParametersProfile GetParametersProfile(this HttpContext context) {
            var profile = new ParametersProfile();
            profile.CurrentContext = context;
            if (!profile.IsCurrentContextEmpty) {
                profile.Request = context.Request;
                profile.Session = context.Session;
                if (!profile.IsRequestEmpty) {
                    profile.Form = context.Request.Form;
                    profile.Params = context.Request.Params;
                    profile.RequestCookies = context.Request.Cookies;
                }
                profile.Response = context.Response;
                if (!profile.IsResponseEmpty) {
                    profile.ResponseCookies = context.Response.Cookies;
                }
            }
            return profile;
        }
    }
}