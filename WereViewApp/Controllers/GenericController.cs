#region using block

using System;
using System.Web.Mvc;
using DevMvcComponent.Error;
using WeReviewApp.Modules.Extensions.Context;

#endregion

namespace WeReviewApp.Controllers {
    internal abstract class GenericController<TContext> : Controller where TContext : DevDbContext, new(){
        internal readonly TContext db;
        internal ErrorCollector ErrorCollector;

        protected GenericController() {
        }

        protected GenericController(bool applicationDbContextRequried) {
            if (applicationDbContextRequried) {
                db = new TContext();
            }
        }

        protected GenericController(bool applicationDbContextRequried, bool errorCollectorRequried) {
            if (errorCollectorRequried) {
                ErrorCollector = new ErrorCollector();
            }
            if (applicationDbContextRequried) {
                db = new TContext();
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
            GC.Collect();
        }
    }
}