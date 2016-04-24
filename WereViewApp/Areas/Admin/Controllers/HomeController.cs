using System.Web.Mvc;
using System.Web.UI;
using WeReviewApp.BusinessLogics;
using WeReviewApp.Models.ViewModels;

namespace WeReviewApp.Areas.Admin.Controllers {
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