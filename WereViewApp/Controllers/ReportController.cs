#region using block

using System.Web.Mvc;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Role;
using WereViewApp.WereViewAppCommon;

#endregion

namespace WereViewApp.Controllers {

    [Authorize]
    public class ReportController : AdvanceController {
        #region Constants and variables

        const string DeletedError = "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";
        const string DeletedSaved = "Removed successfully.";
        const string EditedSaved = "Modified successfully.";
        const string EditedError = "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";
        const string CreatedError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        const string CreatedSaved = "Transaction is successfully added to the database.";
        const string ControllerName = "Report";
        ///Constant value for where the controller is actually visible.
        const string ControllerVisibleUrl = "/Report/";
        const string CurrentControllerRemoveOutputCacheUrl = "/Partials/ReportID";
        const string DynamicLoadPartialController = "/Partials/";
        bool DropDownDynamic = true;
        #endregion


        public ReportController() : base(true) {
            ViewBag.visibleUrl = ControllerVisibleUrl;
            ViewBag.dropDownDynamic = DropDownDynamic;
            ViewBag.dynamicLoadPartialController = DynamicLoadPartialController;
        }
      


        public void SetDefaults() {
            
        }


        public ActionResult App(long id) {
            if (RoleManager.IsInRole(RoleNames.Rookie) == false) {
               return AppVar.GetAuthenticationError("Unauthorized", "");
            }
            var app = db.Apps.Find(id);
            if (app != null) {
                ViewBag.id = id;
                return View();
            }
            return View("_404");
        }

        public ActionResult Review(long id) {
            if (RoleManager.IsInRole(RoleNames.Rookie) == false) {
                return AppVar.GetAuthenticationError("Unauthorized", "");
            }
            var review = db.Reviews.Find(id);
            if (review != null) {
                return View();
            }
            return View("_404");
        }
    }
}