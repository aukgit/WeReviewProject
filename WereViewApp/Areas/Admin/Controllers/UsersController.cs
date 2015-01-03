﻿using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Controllers;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.Role;
using WereViewApp.Models.ViewModels;
using WereViewApp.Modules.Mail;

namespace WereViewApp.Areas.Admin.Controllers {
    public class UsersController : BasicController {

        #region Developer Comments - Alim Ul karim
        // Generated by Alim Ul Karim on behalf of Developers Organism.
        // Find us developers-organism.com
        // https://www.facebook.com/DevelopersOrganism
        // mailto:alim@developers-organism.com		
        #endregion

        #region Constants

        const string _deletedError = "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";
        const string _deletedSaved = "Removed successfully.";
        const string _editedSaved = "Modified successfully.";
        const string _editedError = "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";
        const string _createdError = "Sorry for the inconvenience, couldn't create the last transaction record.";
        const string _createdSaved = "Transaction is successfully added to the database.";
        const string _controllerName = "Users";
        ///Constant value for where the controller is actually visible.
        const string _controllerVisibleUrl = "";

        #endregion

        #region Enums

        internal enum ViewStates {
            Index,
            Create,
            CreatePost,
            Edit,
            EditPost,
            Details,
            Delete,
            DeletePost,
            FilterBlockedUsers
        }

        #endregion

        #region Constructors

        public UsersController()
            : base(true) {
            ViewBag.controller = _controllerName;
        }

        #endregion

        #region View tapping
        /// <summary>
        /// Always tap once before going into the view.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="ApplicationUser">Gives the model if it is a editing state or creating posting state or when deleting.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        bool ViewTapping(ViewStates view, ApplicationUser applicationUser = null) {
            switch (view) {
                case ViewStates.Index:
                    break;
                case ViewStates.Create:
                    break;
                case ViewStates.CreatePost: // before saving it
                    break;
                case ViewStates.Edit:
                    break;
                case ViewStates.Details:
                    break;
                case ViewStates.EditPost: // before saving it
                    break;
                case ViewStates.Delete:
                    break;
            }
            return true;
        }
        #endregion

        #region Save database common method

        /// <summary>
        /// Better approach to save things into database(than db.SaveChanges()) for this controller.
        /// </summary>
        /// <param name="ViewStates">Say the view state, where it is calling from.</param>
        /// <param name="ApplicationUser">Your model information to send in email to developer when failed to save.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
        bool SaveDatabase(ViewStates view, ApplicationUser applicationUser = null) {
            // working those at HttpPost time.
            switch (view) {
                case ViewStates.Create:
                    break;
                case ViewStates.Edit:
                    break;
                case ViewStates.Delete:
                    break;
            }

            try {
                var changes = db.SaveChanges(applicationUser);
                if (changes > 0) {
                    return true;
                }
            } catch (Exception ex) {
                throw new Exception("Message : " + ex.Message.ToString() + " Inner Message : " + ex.InnerException.Message.ToString());
            }
            return false;
        }
        #endregion

        #region DropDowns Generate

        public void GetDropDowns(ApplicationUser applicationUser) {
            ViewBag.CountryID = new SelectList(db.Countries.Where(n => n.CountryID == applicationUser.CountryID).ToList(), "CountryID", "CountryName", applicationUser.CountryID);
            ViewBag.UserTimeZoneID = new SelectList(db.UserTimeZones.Where(n => n.UserTimeZoneID == applicationUser.UserTimeZoneID).ToList(), "UserTimeZoneID", "UTCName", applicationUser.UserTimeZoneID);
            ViewBag.CountryLanguageID = new SelectList(db.CountryLanguages.Where(n => n.CountryLanguageID == applicationUser.CountryLanguageID).ToList(), "CountryLanguageID", "Language", applicationUser.CountryLanguageID);
        }

        public void GetDropDowns(long id) {

        }
        #endregion

        #region Index
        public ActionResult Index() {
            bool viewOf = ViewTapping(ViewStates.Index);
            return View(db.Users.ToList());
        }
        #endregion

        #region Index Find - Commented

        public ActionResult FilterBlockedUsers() {
            bool viewOf = ViewTapping(ViewStates.FilterBlockedUsers);
            return View("FilterBlockedUsers", db.Users.Where(n => n.IsBlocked).ToList());
        }

        #endregion

        #region Manage roles
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        public ActionResult ManageRoles(long id) {
            var user = UserManager.GetUser(id);

            var ManageRole = new ManageRolesViewModel();
            ManageRole.AllRoles = RoleManager.GetRoles();
            ManageRole.UserInRoles = RoleManager.GetUserRolesAsApplicationRole(id).ToList();
            ManageRole.UserDisplayName = user.DisplayName;
            ManageRole.UserId = user.UserID;

            return View(ManageRole);
        }

        #region Add Roles
        public ActionResult AddRole(long userId, long roleId) {
            RoleManager.AddRoleToUser(userId, roleId);
            return RedirectToAction("ManageRoles", new { id = userId });
        }
        #endregion

        #region Removing Roles
        public ActionResult RemoveRole(long userId, long roleId) {
            RoleManager.RemoveUserRole(userId, roleId);
            return RedirectToAction("ManageRoles", new { id = userId });
        }
        #endregion


        #endregion

        #region Block user
        public ActionResult UserBlock(long id) {
            var user = UserManager.GetUser(id);
            if (user != null) {
                var block = new UserBlockViewModel() {
                    UserId = id,
                    UserName = user.DisplayName
                };
                return View(block);
            }
            return View("Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserBlock(UserBlockViewModel model) {
            var user = db.Users.Find(model.UserId);
            if (user != null) {
                //same user
                user.IsBlocked = true;
                user.BlockedbyUserId = UserManager.GetLoggedUserId();
                db.Entry(user).State = EntityState.Modified;
                if (SaveDatabase(ViewStates.EditPost, user)) {
                    string currentUserName = UserManager.GetCurrentUser().DisplayName;

                    string mailBody = MailHtml.BlockEmailHtml(user, model.Reason, currentUserName);

                    AppVar.Mailer.Send(user.Email, "You have been blocked.", mailBody, "Blocking");

                    return RedirectToAction("Index");
                } else {
                    AppVar.SetErrorStatus(ViewBag, _editedError);
                    return View(user);
                }
            }
            AppVar.SetErrorStatus(ViewBag, _editedError);
            return View(user);
        }

        public ActionResult EnableUserBlock(long id) {
            var user = UserManager.GetUser(id);
            if (user != null) {
                var block = new UserBlockViewModel() {
                    UserId = id,
                    UserName = user.DisplayName
                };
                return View(block);
            }
            return View("Error");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnableUserBlock(UserBlockViewModel model) {
            var user = db.Users.Find(model.UserId);
            if (user != null) {
                //same user
                user.IsBlocked = false;
                user.BlockedbyUserId = 0;
                user.BlockingReason = "";
                db.Entry(user).State = EntityState.Modified;
                if (SaveDatabase(ViewStates.EditPost, user)) {
                    string currentUserName = UserManager.GetCurrentUser().DisplayName;

                    string mailBody = MailHtml.ReleasedFromBlockEmailHtml(user, currentUserName, true);

                    AppVar.Mailer.Send(user.Email, "You have been re-enabled.", mailBody, "Enabled");

                    return RedirectToAction("Index");
                } else {
                    AppVar.SetErrorStatus(ViewBag, _editedError);
                    return View(user);
                }
            }
            AppVar.SetErrorStatus(ViewBag, _editedError);
            return View(user);
        }

        #endregion

        #region Details
        public ActionResult Details(long id) {
            var applicationUser = db.Users.Find(id);
            if (applicationUser == null) {
                return HttpNotFound();
            }
            GetDropDowns(applicationUser);
            bool viewOf = ViewTapping(ViewStates.Details, applicationUser);
            return View(applicationUser);
        }
        #endregion

        #region Removing output cache
        public void RemoveOutputCache(string url) {
            HttpResponse.RemoveOutputCacheItem(url);
        }
        #endregion
    }


}
