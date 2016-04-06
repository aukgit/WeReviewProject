//using WereViewApp.Filter;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Filter;

namespace WereViewApp {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new AreaAuthorizeAttribute());
        }
    }
}
