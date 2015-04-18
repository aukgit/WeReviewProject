#region using block

using System.Web.Mvc;
using WereViewApp.Filter;
using WereViewApp.Models.EntityModel;
using WereViewApp.Modules.UserError;

#endregion

namespace WereViewApp.Controllers {
    //[CompressFilter]
    [CacheFilter(Duration = 8)]
    public abstract class AdvanceController : Controller {
        internal ErrorCollector ErrorCollector;
        internal readonly WereViewAppEntities db;

        public AdvanceController() {
        }

        public AdvanceController(bool applicationDbContextRequried) {
            if (applicationDbContextRequried) {
                db = new WereViewAppEntities();
            }
        }

        public AdvanceController(bool applicationDbContextRequried, bool errorCollectorRequried) {
            if (errorCollectorRequried) {
                ErrorCollector = new ErrorCollector();
            }
            if (applicationDbContextRequried) {
                db = new WereViewAppEntities();
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