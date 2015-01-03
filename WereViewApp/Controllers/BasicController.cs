using WereViewApp.Models.Context;
using WereViewApp.Modules.UserError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Filter;

namespace WereViewApp.Controllers {

    //[CompressFilter(Order = 1)]

    public abstract class BasicController : Controller {
        internal readonly ApplicationDbContext db;
        internal ErrorCollector _errorCollector;


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