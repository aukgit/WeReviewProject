//using WeReviewApp.Filter;
using System.Web;
using System.Web.Mvc;
using WeReviewApp.Filter;

namespace WeReviewApp {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new AreaAuthorizeAttribute());
        }
    }
}
