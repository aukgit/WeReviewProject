using WereViewApp.Models.Context;
using WereViewApp.Modules.UserError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Models.EntityModel;
using WereViewApp.Filter;

namespace WereViewApp.Controllers {

    //[CompressFilter]
    [CacheFilter(Duration = 8)]
    public abstract class AdvanceController : Controller {
        internal readonly WereViewAppEntities db;
        internal ErrorCollector _errorCollector;


        public AdvanceController() {

        }
        public AdvanceController(bool applicationDbContextRequried) {
            if (applicationDbContextRequried) {
                db = new WereViewAppEntities();
            }
        }
        public AdvanceController(bool applicationDbContextRequried, bool errorCollectorRequried) {
            if (errorCollectorRequried) {
                _errorCollector = new ErrorCollector();
            }
            if (applicationDbContextRequried) {
                db = new WereViewAppEntities();
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