using System.Collections.Specialized;
using System.Web;

namespace WeReviewApp.Modules.Type {
    public class ParametersProfile {
        public NameValueCollection Params { get; set; }
        public HttpRequestBase Request { get; set; }
        public HttpResponseBase Response { get; set; }
        public HttpSessionStateBase Session { get; set; }
        public HttpCookieCollection ResponseCookies { get; set; }
        public HttpCookieCollection RequestCookies { get; set; }
        public NameValueCollection Form { get; set; }
        public HttpContextBase CurrentContext { get; set; }

        public bool IsRequestEmpty {
            get { return Request == null; }
        }

        public bool IsResponseCookiesEmpty {
            get { return ResponseCookies == null; }
        }

        public bool IsRequestCookiesEmpty {
            get { return RequestCookies == null; }
        }
        public bool IsFormEmpty {
            get { return Form == null; }
        }
        public bool IsResponseEmpty {
            get { return Response == null; }
        }

        public bool IsSessionEmpty {
            get { return Session == null; }
        }

        public bool IsCurrentContextEmpty {
            get { return CurrentContext == null; }
        }


    }
}