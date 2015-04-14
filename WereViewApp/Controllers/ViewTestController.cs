using System.Web.Mvc;

namespace WereViewApp.Controllers
{
    public class ViewTestController : Controller
    {
        // GET: ViewTest
        public ActionResult Index(string v)
        {
            return View(v);
        }
    }
}