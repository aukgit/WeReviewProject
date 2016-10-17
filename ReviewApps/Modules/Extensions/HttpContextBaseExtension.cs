using System.Web;
using ReviewApps.Modules.Type;

namespace ReviewApps.Modules.Extensions {
    public static class HttpContextBaseExtension {
        public static ParametersProfileBase GetParametersProfile(this HttpContextBase context) {
            var profile = new ParametersProfileBase();
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