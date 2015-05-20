#region using block

using System.Web.Mvc;
using WereViewApp.WereViewAppCommon;

#endregion

namespace WereViewApp.Controllers {

    [Authorize]
    public class ReportController : AdvanceController {

        public ReportController() : base(true) {
            
        }
      

        public void SetDefaults() {
            
        }
        public ActionResult App(long id) {
            var app = db.Apps.Find(id);
            if (app != null) {
                return View();
            }
        }
    }
}