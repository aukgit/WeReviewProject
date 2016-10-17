using System.Web.Mvc;
using System.Web.UI;
using ReviewApps.BusinessLogics;
using ReviewApps.Models.ViewModels;

namespace ReviewApps.Areas.Admin.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public class HomeController : Controller {
        public ActionResult Index() {
            var appSummaryModel = Session["admin-app-summary"] as AppSummaryViewModel;
            if (appSummaryModel == null) {
                var logics = new Logics();
                appSummaryModel = logics.GetAppsSummary();
                Session["admin-app-summary"] = appSummaryModel;
            }
            return View(appSummaryModel);
        }
    }
}