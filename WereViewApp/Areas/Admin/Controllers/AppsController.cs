using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using DevMvcComponent.Pagination;
using WereViewApp.BusinessLogic;
using WereViewApp.BusinessLogic.Admin;
using WereViewApp.Controllers;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Models.ViewModels;
using WereViewApp.Modules.Extensions;
using WereViewApp.Modules.Extensions.IdentityExtension;
using WereViewApp.Modules.Mail;
using WereViewApp.WereViewAppCommon;

namespace WereViewApp.Areas.Admin.Controllers {
    [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
    public class AppsController : AdvanceController {
        public AppsController()
            : base(true) {}

        #region TempData : Get and set from TempData dictionary to perform temporary operations.

        private const string TempAppKey = "app-moderate";
        private const string TempAppFeaturedKey = "app-moderate-is-featured";
        private const string TempAppUserKey = "app-moderate-user";

        private void SetTempData(App app, ApplicationUser developerUser, bool isFeaturedPreviously) {
            TempData[TempAppKey] = app;
            TempData[TempAppUserKey] = developerUser;
            TempData[TempAppFeaturedKey] = isFeaturedPreviously;
        }

        /// <summary>
        ///     Returns true if app is valid.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="developerUser"></param>
        /// <param name="isFeaturedPreviously"></param>
        /// <returns>Returns true if app is valid.</returns>
        private bool GetTempData(out App app, out ApplicationUser developerUser, out bool isFeaturedPreviously) {
            app = TempData[TempAppKey] as App;
            if (app != null) {
                developerUser = TempData[TempAppUserKey] as ApplicationUser;
                isFeaturedPreviously = TempData[TempAppFeaturedKey] != null && (bool) TempData[TempAppFeaturedKey];
                return true;
            }
            developerUser = null;
            isFeaturedPreviously = false;
            return false;
        }

        #endregion

        #region Pagination

        /// <summary>
        /// </summary>
        /// <param name="apps"></param>
        /// <param name="paginationUrl">"url/@page"</param>
        /// <returns></returns>
        private List<App> GetPagedApps(IQueryable<App> apps, string paginationUrl, int? page = 1) {
            if (!page.HasValue) {
                page = 1;
            }
            apps = apps.OrderByDescending(n => n.AppID);
            var paginationInfo = new PaginationInfo() {
                ItemsInPage = AppVar.Setting.PageItems,
                PageNumber = page.Value,
            };
            var pagedApps = apps.GetPageData(paginationInfo).ToList();
            ViewBag.paginationHtml =
                new MvcHtmlString(Pagination.GetList(paginationInfo, paginationUrl, "Apps of Page : @page"));
            return pagedApps;
        }

        /// <summary>
        /// </summary>
        /// <param name="apps"></param>
        /// <param name="paginationUrl">"url/@page"</param>
        /// <returns></returns>
        private List<App> GetPagedApps(IList<App> apps, string paginationUrl, int? page = 1) {
            if (!page.HasValue) {
                page = 1;
            }
            if (apps == null || apps.Count == 0) {
                return new List<App>();
            }
            var paginationInfo = new PaginationInfo() {
                ItemsInPage = AppVar.Setting.PageItems,
                PageNumber = page.Value,
            };
            var pagedApps = apps.GetPageData(paginationInfo).ToList();
            ViewBag.paginationHtml =
                new MvcHtmlString(Pagination.GetList(paginationInfo, paginationUrl, "Apps of Page : @page"));
            return pagedApps;
        }

        #endregion

        // GET: Admin/AppModerate
        public ActionResult Index(int? page = 1, string search = "") {
            if (!page.HasValue) {
                page = 1;
            }
            List<App> apps;

            var url = this.CurrentControlerAbsoluteUrl() + "?page=@page";
            var query = db.Apps.Include(n => n.User).Include(n => n.FeaturedImages);

            if (!string.IsNullOrWhiteSpace(search)) {
                url += "&search=" + Server.UrlEncode(search);
                var algorithms = new Logics();
                query = algorithms.GetSimpleAppSearchResults(query, search);
                query = query.OrderByDescending(n => n.AppID);
                apps = GetPagedApps(query, url, page);
                ViewBag.Search = search;
            } else {
                query = query.OrderByDescending(n => n.AppID);
                apps = GetPagedApps(query, url, page);
            }
            return View(apps);
        }

        #region Moderate

        // GET: Admin/AppModerate
        public ActionResult Moderate(long id) {
            var appModerateModel = new AppModerateViewModel() {
                AppId = id
            };

            var app = db.Apps.Find(id);
            var developerUser = User.GetUser(app.PostedByUserID);
            ViewBag.user = developerUser;

            if (app == null) {
                return HttpNotFound();
            }
            appModerateModel.App = app;
            appModerateModel.IsBlocked = app.IsBlocked;
            appModerateModel.IsFeatured = db.FeaturedImages.Any(n => n.IsFeatured && n.AppID == id);
            // set temp data
            SetTempData(app, developerUser, appModerateModel.IsFeatured);
            return View(appModerateModel);
        }

        [HttpPost]
        public ActionResult Moderate(AppModerateViewModel model) {
            App app;
            ApplicationUser developerUser;
            bool isFeaturedPreviously;
            if (!GetTempData(out app, out developerUser, out isFeaturedPreviously)) {
                // if app not found
                return RedirectToAction("Index");
            }

            SetTempData(app, developerUser, isFeaturedPreviously);
            model.App = app;
            if (app != null) {
                ViewBag.user = developerUser;
                if (app.IsBlocked != model.IsBlocked) {
                    // needs to update
                    if (model.IsBlocked) {
                        ModerationLogics.BlockApp(model.AppId, true, db);
                    } else {
                        ModerationLogics.UnBlockApp(model.AppId, true, db);
                    }
                }
                if (isFeaturedPreviously != model.IsFeatured) {
                    // needs to update
                    ModerationLogics.AppFeatured(model.AppId, model.IsFeatured, true, db);
                }
                var statusMessage = "You have successfully moderated '" + app.AppName + "' app.";

                if (!string.IsNullOrWhiteSpace(model.Message)) {
                    SendEmailToAppDeveloper(developerUser, model);
                    statusMessage += " An email is also sent!";
                }
                AppVar.SetSavedStatus(ViewBag, statusMessage);
                return View(model);
            }
            AppVar.SetErrorStatus(ViewBag, "Sorry last transaction has been failed.");
            return View(model);
        }

        #endregion

        #region Send email/ message to Developer

        private void SendEmailToAppDeveloper(ApplicationUser developerUser, AppModerateViewModel model) {
            var loggedUser = User.GetUser();
            var loggedUsername = loggedUser.DisplayName;
            var sb = new StringBuilder(50);
            MailHtml.AddGreetingsToStringBuilder(developerUser, sb);
            sb.AppendLine(MailHtml.LineBreak);
            sb.AppendLine(model.Message);
            sb.AppendLine(MailHtml.LineBreak);
            if (model.LikeToHearFromYou) {
                sb.AppendLine(MailHtml.LineBreak);
                sb.AppendLine(
                    MailHtml.GetStrongTag(
                        "Note: ** We would like to hear back from you. Please send your replies to '" +
                        AppVar.Setting.OfficeEmail + "' **"));
                sb.AppendLine(MailHtml.LineBreak);
            }
            sb.AppendLine(MailHtml.LineBreak);
            sb.AppendLine();
            MailHtml.AddThanksFooterOnStringBuilder(loggedUser.DisplayName, "Administrator", sb);
            var message = sb.ToString();
            sb = null;
            GC.Collect();
            var subjectToDeveloper = "A message from admin : " + loggedUsername;
            var subjectToAdmin = "An email sent to : " + developerUser.Email + " [this mail contains the sample]";

            AppVar.Mailer.Send(developerUser.Email, subjectToDeveloper, message);
            AppVar.Mailer.Send(loggedUser.Email, subjectToAdmin, message);
        }

        #endregion
    }
}