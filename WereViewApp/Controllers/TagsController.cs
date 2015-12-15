using System.Web.Mvc;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Controllers {
    public class TagsController : AdvanceController {
        #region Declarations

        private readonly Algorithms _algorithms = new Algorithms();

        #endregion

        #region Constructors

        public TagsController()
            : base(true) {
        }

        #endregion
        // GET: Tags
        public ActionResult Index() {
            return View();
        }
    }
}