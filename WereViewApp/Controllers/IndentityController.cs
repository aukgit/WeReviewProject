#region using block

using System;
using System.Web.Mvc;
using DevMvcComponent.Error;
using WeReviewApp.Models.Context;

#endregion

namespace WeReviewApp.Controllers {
    public abstract class IndentityController<TContext> : Controller where TContext : ApplicationDbContext, new() {
        internal readonly TContext db;
        internal ErrorCollector ErrorCollector;

        protected IndentityController() {}

        protected IndentityController(bool applicationDbContextRequried) {
            if (applicationDbContextRequried) {
                db = new TContext();
            }
        }

        protected IndentityController(bool applicationDbContextRequried, bool errorCollectorRequried) {
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