using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace WereViewApp.Modules.Authentication {
    public static class Auth {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static ActionResult IsRoleValid(this Controller controller, string role, string viewName) {
            return View(viewName);
        }
    }
}