using System.IO;
using System.Web.Mvc;

namespace ReviewApps.Modules.Extensions.ViewEngine {
    public class RenderingView {
        /// <summary>
        ///     return view to string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewPath"></param>
        /// <param name="model"></param>
        /// <param name="partial"></param>
        /// <returns></returns>
        private static string RenderViewToString(ControllerContext context, string viewPath, object model = null,
            bool partial = false) {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial) {
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            } else {
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);
            }

            if (viewEngineResult == null) {
                throw new FileNotFoundException("View cannot be found.");
            }

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter()) {
                var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
    }
}