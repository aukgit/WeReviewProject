using System.Web.Mvc;

namespace WeReviewApp.Areas.Admin.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        //public ActionResult Elmah() {
        //    return View();
        //}
    }
}