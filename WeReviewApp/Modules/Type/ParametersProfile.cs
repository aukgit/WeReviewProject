using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;

namespace WeReviewApp.Modules.Type {
    public class ParametersProfile {
        public NameValueCollection Params { get; set; }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpSessionState Session { get; set; }
        public HttpCookieCollection ResponseCookies { get; set; }
        public HttpCookieCollection RequestCookies { get; set; }
        public NameValueCollection Form { get; set; }
        public HttpContext CurrentContext { get; set; }

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