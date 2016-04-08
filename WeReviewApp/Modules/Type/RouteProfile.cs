using System.Collections.Generic;
using System.Web.Mvc;

namespace WereViewApp.Modules.Type {
    public class RouteProfile {
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Action { get; set; }
        public IDictionary<string, object> Parameters { get; set; }

        public ActionDescriptor ActionDescriptor { get; set; }
    }
}