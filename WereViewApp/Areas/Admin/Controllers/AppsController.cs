using System;
using DevMvcComponent.Pagination;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WereViewApp.Controllers;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.ViewModels;
using WereViewApp.WereViewAppCommon;
using WereViewApp.WereViewAppCommon.Structs;
using WereViewApp.Modules.Extensions.IdentityExtension;
using WereViewApp.Modules.Mail;

namespace WereViewApp.Areas.Admin.Controllers {
    public class AppsController : AdvanceController {

        public AppsController()
            : base(true) {

        }

        private const string TempAppKey = "app-moderate";
        private const string TempAppFeaturedKey = "app-moderate-is-featured";
        /// <summary>
        /// 
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
            ViewBag.paginationHtml = new MvcHtmlString(Pagination.GetList(paginationInfo, paginationUrl, "Apps of Page : @page"));
            return pagedApps;
        }
        /// <summary>
        /// 
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
            ViewBag.paginationHtml = new MvcHtmlString(Pagination.GetList(paginationInfo, paginationUrl, "Apps of Page : @page"));
            return pagedApps;
        }
        // GET: Admin/AppModerate
        public ActionResult Index(int? page = 1, string search = "") {
            if (!page.HasValue) {
                page = 1;
            }
            List<App> apps;

            string url = AppVar.Url + "/Admin/Apps?page=@page";
            if (!string.IsNullOrWhiteSpace(search)) {
                url += "&search=" + Server.UrlEncode(search);
                var algorithms = new Algorithms();
                var query = algorithms.GetSearchResults(search, null, null, null, CommonVars.SearchResultsMaxResultReturn);
                apps = GetPagedApps(query, url, page);
                ViewBag.Search = search;
            } else {
                apps = GetPagedApps(db.Apps.Include(n => n.User), url, page);

            }
            return View(apps);
        }


        // GET: Admin/AppModerate
        public ActionResult Moderate(long id) {
            var appModerateModel = new AppModerateViewModel() {
                AppId = id
            };

            var app = db.Apps.Find(id);
            ViewBag.user = User.GetUser(app.PostedByUserID);
            if (app == null) {
                return HttpNotFound();
            }
            TempData[TempAppKey] = app;
            appModerateModel.App = app;
            appModerateModel.IsBlocked = app.IsBlocked;
            appModerateModel.IsFeatured = db.FeaturedImages.Any(n => n.IsFeatured && n.AppID == id);
            TempData[TempAppFeaturedKey] = appModerateModel.IsFeatured;
            return View(appModerateModel);
        }
        [HttpPost]
        public ActionResult Moderate(AppModerateViewModel model) {
            var app = TempData[TempAppKey] as App;
            if (app == null) {
                return RedirectToAction("Index");
            }
            var isFeaturedPreviously = (bool)TempData[TempAppFeaturedKey];
            model.App = app;

            if (app != null) {
                TempData[TempAppKey] = app;
                TempData[TempAppFeaturedKey] = isFeaturedPreviously;

                var user = User.GetUser(app.PostedByUserID);
                var loggedUser = User.GetUser();

                var loggedUsername = loggedUser.FirstName + " " + loggedUser.LastName ;
                ViewBag.user = user;
                if (app.IsBlocked != model.IsBlocked) {
                    // needs to update
                    if (model.IsBlocked) {
                        ModerationAlgorithms.BlockApp(model.AppId, model.IsFeatured, db);
                    } else {
                        ModerationAlgorithms.UnBlockApp(model.AppId, model.IsFeatured, db);
                    }
                }
                if (isFeaturedPreviously != model.IsFeatured) {
                    // needs to update
                    ModerationAlgorithms.AppFeatured(model.AppId, model.IsFeatured, db);
                }
                string statusMessage = "You have successfully moderated '" + app.AppName + "' app.";

                if (!string.IsNullOrWhiteSpace(model.Message)) {
                    var sb = new StringBuilder(50);
                    MailHtml.AddGreetingsToStringBuilder(user, sb);
                    sb.AppendLine(MailHtml.LineBreak);
                    sb.AppendLine(model.Message);
                    sb.AppendLine(MailHtml.LineBreak);
                    if (model.LikeToHearFromYou) {
                        sb.AppendLine(MailHtml.LineBreak);
                        sb.AppendLine("** We surely like to hear back from you. **");
                        sb.AppendLine(MailHtml.LineBreak);
                    }
                    sb.AppendLine(MailHtml.LineBreak);
                    sb.AppendLine();
                    MailHtml.AddThanksFooterOnStringBuilder(loggedUsername, "Administrator", sb);
                    var message = sb.ToString();
                    sb = null;
                    GC.Collect();
                    AppVar.Mailer.Send(user.Email, "A message from admin : " + loggedUsername, message);
                    statusMessage += " An email is also sent!";
                    AppVar.Mailer.Send(loggedUser.Email, "An email sent to : " + user.Email + " [this mail contains the sample]", message);
                }
                AppVar.SetSavedStatus(ViewBag, statusMessage);
                return View(model);
            }



            AppVar.SetErrorStatus(ViewBag, "Sorry last transaction has been failed.");
            return View(model);
        }
    }
}