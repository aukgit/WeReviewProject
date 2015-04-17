using System.Web.Mvc;
using WereViewApp.Models.Context;
using WereViewApp.Modules.UserError;

namespace WereViewApp.Controllers {
    //[CompressFilter(Order = 1)]

    public abstract class BasicController : Controller {
        internal ErrorCollector _errorCollector;
        internal readonly ApplicationDbContext db;

        public BasicController() {
        }

        public BasicController(bool applicationDbContextRequried) {
            if (applicationDbContextRequried) {
                db = new ApplicationDbContext();
            }
        }

        public BasicController(bool applicationDbContextRequried, bool errorCollectorRequried) {
            if (errorCollectorRequried) {
                _errorCollector = new ErrorCollector();
            }
            if (applicationDbContextRequried) {
                db = new ApplicationDbContext();
            }
        }

        protected override void Dispose(bool disposing) {
            if (db != null) {
                db.Dispose();
            }
            if (_errorCollector != null) {
                _errorCollector.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}