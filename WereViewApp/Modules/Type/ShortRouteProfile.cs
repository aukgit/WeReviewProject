using System.Collections.Generic;
using System.Web.Mvc;
using WeReviewApp.Modules.Extensions;

namespace WeReviewApp.Modules.Type {
    public class ShortRouteProfile {
        public ShortRouteProfile() {}

        public ShortRouteProfile(ActionExecutingContext context) {
            Action = context.GetAreaName();
            Controller = context.GetControllerName();
            Area = context.GetAreaName();
        }

        public string Controller { get; set; }
        public string Area { get; set; }
        public string Action { get; set; }

        /// <summary>
        ///     Get Action + joiner + Controller + joiner + Area
        /// </summary>
        /// <param name="joiner"></param>
        /// <returns></returns>
        public string GetCombined(string joiner) {
            return Action + joiner + Controller + joiner + Area;
        }
    }
}