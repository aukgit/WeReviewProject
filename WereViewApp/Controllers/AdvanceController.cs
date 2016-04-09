#region using block

using System.Web.Mvc;
using DevMvcComponent.Error;
using WeReviewApp.Models.EntityModel;

#endregion

namespace WeReviewApp.Controllers {
    //[CompressFilter]
    //[CacheFilter(Duration = 3600)]
    public abstract class AdvanceController : Controller {
        internal ErrorCollector ErrorCollector;
        internal readonly WereViewAppEntities db;

        protected AdvanceController() {
        }

        protected AdvanceController(bool dbContextRequried) {
            if (dbContextRequried) {
                db = new WereViewAppEntities();
            }
        }

        protected AdvanceController(bool dbContextRequried, bool errorCollectorRequried) {
            if (errorCollectorRequried) {
                ErrorCollector = new ErrorCollector();
            }
            if (dbContextRequried) {
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