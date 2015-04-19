#region using block

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WereViewApp.Models.EntityModel;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Session;
using WereViewApp.WereViewAppCommon.Structs;

#endregion

namespace WereViewApp.Controllers {
    public class ValidatorController : Controller {
        #region WereView Validators

        #region App URL Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetValidUrl(App app) {
            var max = 60;
            var min = 3;
            var maxTry = 250;

            var id = app.AppName;

            try {
                if (id == null || id.Length < 5) {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                if (!AppVar.Setting.IsInTestingEnvironment) {
                    if (SessionNames.IsValidationExceed("GetValidUrl", maxTry)) {
                        throw new Exception("Exceed the limit of try");
                    }
                }

                if ((id.Length >= min && id.Length <= max)) {
                    var url = GetFriendlyURLFromString(id);
                    using (var db = new WereViewAppEntities()) {
                        var exist =
                            db.Apps.Any(
                                n =>
                                    n.PlatformID == app.PlatformID && n.CategoryID == app.CategoryID && n.URL == url &&
                                    n.PlatformVersion == app.PlatformVersion);
                        return Json(!exist, JsonRequestBehavior.AllowGet); // return true;
                    }
                }
            } catch (Exception ex) {
                AppVar.Mailer.HandleError(ex, "Validate GetValidUrl App Posting");
            }
            //found e false
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region App URL Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetValidUrlEditing(App app) {
            var max = 60;
            var min = 3;
            var id = app.AppName;

            try {
                if (id == null || id.Length < 5) {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                if (!AppVar.Setting.IsInTestingEnvironment) {
                    if (SessionNames.IsValidationExceed("GetValidUrl")) {
                        throw new Exception("Exceed the limit of try");
                    }
                }

                if ((id.Length >= min && id.Length <= max)) {
                    var url = GetFriendlyURLFromString(id);
                    if (app.URL.Equals(url)) {
                        return Json(true, JsonRequestBehavior.AllowGet); // return true;
                    }
                    using (var db = new WereViewAppEntities()) {
                        var exist =
                            db.Apps.Any(
                                n =>
                                    n.AppID != app.AppID && n.PlatformID == app.PlatformID &&
                                    n.CategoryID == app.CategoryID && n.URL == url &&
                                    n.PlatformVersion == app.PlatformVersion);
                        return Json(!exist, JsonRequestBehavior.AllowGet); // return true;
                    }
                }
            } catch (Exception ex) {
                AppVar.Mailer.HandleError(ex, "Validate GetValidUrl App-Editing");
            }
            //found e false
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion

        private string GetFriendlyURLFromString(string title) {
            if (!string.IsNullOrEmpty(title)) {
                title = title.Trim();
                title = Regex.Replace(title, CommonVars.FriendlyUrlRegex, "-");
                return title;
            }
            return title;
        }

        #endregion

        #region Base Validators

        [HttpPost]
        [OutputCache(CacheProfile = "Long", VaryByParam = "id", VaryByCustom = "byuser")]
        [ValidateAntiForgeryToken]
        public ActionResult GetUsername(string id, string __RequestVerificationToken) {
            var returnParam = true;
            var max = 30;
            var min = 3;
            try {
                if (id == null || id.Length < 3) {
                    return Json(!returnParam, JsonRequestBehavior.AllowGet);
                }
                if (!AppVar.Setting.IsInTestingEnvironment) {
                    if (SessionNames.IsValidationExceed("username")) {
                        throw new Exception("Exceed the limit of try");
                    }
                }
                var userPattern = "^([A-Za-z]|[A-Za-z0-9_.]+)$";
                if (Regex.IsMatch(id, userPattern, RegexOptions.Compiled) && (id.Length >= min && id.Length <= max)) {
                    if (UserManager.IsUserNameExist(id)) {
                        return Json(returnParam, JsonRequestBehavior.AllowGet);
                    }
                    return Json(!returnParam, JsonRequestBehavior.AllowGet); // only true
                }
            } catch (Exception ex) {
                AppVar.Mailer.HandleError(ex, "Validate Username");
            }
            //found e false
            return Json(!returnParam, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OutputCache(CacheProfile = "Long", VaryByParam = "id", VaryByCustom = "byuser")]
        [ValidateAntiForgeryToken]
        public ActionResult Username(string id, string __RequestVerificationToken) {
            var returnParam = true;
            var max = 30;
            var min = 3;
            try {
                if (id == null || id.Length < 3) {
                    return Json(!returnParam, JsonRequestBehavior.AllowGet);
                }
                if (!AppVar.Setting.IsInTestingEnvironment) {
                    if (SessionNames.IsValidationExceed("username")) {
                        throw new Exception("Exceed the limit of try");
                    }
                }
                var userPattern = "^([A-Za-z]|[A-Za-z0-9_.]+)$";
                if (Regex.IsMatch(id, userPattern, RegexOptions.Compiled) && (id.Length >= min && id.Length <= max)) {
                    if (UserManager.IsUserNameExist(id)) {
                        return Json(!returnParam, JsonRequestBehavior.AllowGet);
                    }
                    return Json(returnParam, JsonRequestBehavior.AllowGet); // only true
                }
            } catch (Exception ex) {
                AppVar.Mailer.HandleError(ex, "Validate Username");
            }
            //found e false
            return Json(!returnParam, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OutputCache(CacheProfile = "Long", VaryByParam = "id", VaryByCustom = "byuser")]
        [ValidateAntiForgeryToken]
        public ActionResult Email(string id, string __RequestVerificationToken) {
            if (!AppVar.Setting.IsInTestingEnvironment) {
                if (SessionNames.IsValidationExceed("Email")) {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            try {
                if (id == null || id.Length < 5)
                    return Json(false, JsonRequestBehavior.AllowGet);

                var email = id;

                var emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                if (Regex.IsMatch(email, emailPattern)) {
                    if (!UserManager.IsEmailExist(email)) {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                AppVar.Mailer.HandleError(ex, "Validate Email");

                return Json(false);
            }
        }

        #endregion
    }
}