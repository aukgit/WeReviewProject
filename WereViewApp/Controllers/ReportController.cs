#region using block

using System.Web.Mvc;

#endregion

namespace WereViewApp.Controllers {
    public class ReportController : Controller {
        // GET: Report
        public ActionResult Index() {
            return View();
        }
    }
}