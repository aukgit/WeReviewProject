using System.Web.Mvc;
using System.Web.UI;

namespace WereViewApp.Areas.Admin.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}