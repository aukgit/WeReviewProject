#region using block

using System.Web.Mvc;

#endregion

namespace WereViewApp.Controllers {
    [OutputCache(CacheProfile = "YearNoParam")]
    public class ErrorsController : Controller {
        // GET: Errors
        public ActionResult Index() {
            return View("_Error");
        }

        public ActionResult Error(int number, string title, string reason) {
            ViewBag.title = title;
            ViewBag.Reason = reason;

            return View("_" + number);
        }

        public ActionResult Error_400() {
            return View("_400");
        }

        public ActionResult Error_403() {
            return View("_403");
        }

        public ActionResult Error_404() {
            return View("_404");
        }

        public ActionResult Error_414() {
            return View("_414");
        }

        public ActionResult Error_415() {
            return View("_415");
        }

        public ActionResult Error_429() {
            return View("_429");
        }

        public ActionResult Error_431() {
            return View("_429");
        }

        public ActionResult Error_451() {
            return View("_451");
        }

        public ActionResult Error_500() {
            return View("_500");
        }

        public ActionResult Error_502() {
            return View("_502");
        }
    }
}