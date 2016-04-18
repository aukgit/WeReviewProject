#region using block

using DevMvcComponent.Error;
using System;
using System.Web.Mvc;
using System.Web.UI;
using WeReviewApp.Models.Context;

#endregion

namespace WeReviewApp.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public abstract class IdentityController<TContext> : Controller where TContext : ApplicationDbContext, new() {
        internal readonly TContext db;
        internal ErrorCollector ErrorCollector;

        protected IdentityController() {}

        protected IdentityController(bool applicationDbContextRequried) {
            if (applicationDbContextRequried) {
                db = new TContext();
            }
        }

        protected IdentityController(bool applicationDbContextRequried, bool errorCollectorRequried) {
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