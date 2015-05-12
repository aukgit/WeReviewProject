#region using block

using System.Web.Mvc;
using WereViewApp.WereViewAppCommon;

#endregion

namespace WereViewApp.Controllers {
    public class ReportController : AdvanceController {

        public ReportController() : base(true) {
            
        }
        // GET: Report

        public ActionResult Index() {
            return View();
        }

        public ActionResult App(long id) {
            var app = db.Apps.Find(id);
            if (app != null) {
                return View();
            }
        }
    }
}