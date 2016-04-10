using System.Collections.Generic;
using System.Web.Mvc;

namespace WeReviewApp.Modules.Type {
    public class RouteProfile : ShortRouteProfile {
        public RouteProfile() {}

        public RouteProfile(ActionExecutingContext context) : base(context) {
            ActionParameters = context.ActionParameters;
            ActionDescriptor = context.ActionDescriptor;
        }

        public IDictionary<string, object> ActionParameters { get; set; }

        public ActionDescriptor ActionDescriptor { get; set; }
    }
}