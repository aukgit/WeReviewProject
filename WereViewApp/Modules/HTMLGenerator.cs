using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WereViewApp.Modules {
    public static class HTMLGenerator {

        public static MvcHtmlString GetHidden(this Controller ctn, string name, string value = null) {
            return new MvcHtmlString(string.Format("<input type='hidden' name='{0}' id='{0}' value='{1}'", name, value));
        }
    }
}