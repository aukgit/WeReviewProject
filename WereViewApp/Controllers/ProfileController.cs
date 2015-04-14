using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Modules.DevUser;

namespace WereViewApp.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile/username
        public ActionResult Index(string username) {
            var user = UserManager.GetUser(username);
            if (user != null) {
                
            }
            return View();
        }
    }
}