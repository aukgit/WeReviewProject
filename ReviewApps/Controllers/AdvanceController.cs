#region using block

using System.Web.Mvc;
using System.Web.UI;
using DevMvcComponent.Error;
using ReviewApps.Models.EntityModel;

#endregion

namespace ReviewApps.Controllers {
    //[CompressFilter]
    //[CacheFilter(Duration = 3600)]
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public abstract class AdvanceController : Controller {
        internal readonly ReviewAppsEntities db;
        internal ErrorCollector ErrorCollector;
        protected string ControllerUrl = "";

        protected AdvanceController() {}

        protected AdvanceController(bool dbContextRequried) {
            if (dbContextRequried) {
                db = new ReviewAppsEntities();
            }
        }

        protected AdvanceController(bool dbContextRequried, bool errorCollectorRequried) {
            if (errorCollectorRequried) {
                ErrorCollector = new ErrorCollector();
            }
            if (dbContextRequried) {
                db = new ReviewAppsEntities();
            }
        }

        protected override void Dispose(bool disposing) {
            if (db != null) {
                db.Dispose();
            }
            if (ErrorCollector != null) {
                ErrorCollector.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}