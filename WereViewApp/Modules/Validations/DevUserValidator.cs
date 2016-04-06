using System;
using System.Linq;
using DevMvcComponent.Error;
using Microsoft.AspNet.Identity;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.Identity;
using WereViewApp.Models.ViewModels;
using WereViewApp.Modules.DevUser;
using WereViewApp.Modules.UserError;

namespace WereViewApp.Modules.Validations {
    public class DevUserValidator : Validator {
        private readonly RegisterViewModel _viewMdoel;
        private readonly ApplicationDbContext db;

        public DevUserValidator(RegisterViewModel viewMdoel, ErrorCollector errorCollector, ApplicationDbContext dbContext)
            : base(errorCollector) {
            _viewMdoel = viewMdoel;
            db = dbContext;
        }
        /// <summary>
        /// Validate register code.
        /// Returns true means validation is correct.
        /// </summary>
        /// <returns></returns>
        public bool RegisterCodeValidate() {
            if (!AppVar.Setting.IsRegisterCodeRequiredToRegister) {
                _viewMdoel.RegistraterCode = Guid.NewGuid();
                _viewMdoel.Role = -1;
            } else {
                var regCode = db.RegisterCodes.FirstOrDefault(n => n.IsUsed == false && n.RoleID == _viewMdoel.Role && n.RegisterCodeID == _viewMdoel.RegistraterCode && !n.IsExpired);
                if (regCode != null) {
                    if (regCode.ValidityTill <= DateTime.Now) {
                        // not valid
                        regCode.IsExpired = true;
                        ErrorCollector.AddMedium(MessageConstants.RegistercCodeExpired, "", "", "", MessageConstants.SolutionContactAdmin);
                        db.SaveChanges();
                        return false;
                    }
                } else {
                    ErrorCollector.AddMedium(MessageConstants.RegistercCodeNotValid, "", "", "", MessageConstants.SolutionContactAdmin);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Is user already exist.
        /// </summary>
        /// <returns></returns>
        public bool IsUserDoesntExist() {
            if (AppVar.Setting.IsFirstUserFound) {
                // first user is already registered.
                ApplicationUser user;
                if (UserManager.IsUserNameExistWithValidation(_viewMdoel.UserName, out user)) {
                    ErrorCollector.AddHigh(MessageConstants.UserNameExist, "", "", "", MessageConstants.SolutionContactAdmin);
                    return false; // not valid
                }
            } 
            return true;
        }

        /// <summary>
        /// Is email already exist.
        /// </summary>
        /// <returns></returns>
        public bool IsEmailDoesntExist() {
            if (AppVar.Setting.IsFirstUserFound) {
                // first user is already registered.
                ApplicationUser user;
                if (UserManager.IsEmailExist(_viewMdoel.Email)) {
                    ErrorCollector.AddHigh(MessageConstants.UserNameExist, "", "", "", MessageConstants.SolutionContactAdmin);
                    return false; // not valid
                }
            }
            return true;
        }

        /// <summary>
        /// Validate Language
        /// </summary>
        /// <returns></returns>
        //public bool LanguageValidate() {
        //    var languages = CachedQueriedData.GetLanguages(_viewMdoel.CountryID, 0);
        //    if (languages == null) {
        //        //select English as default.
        //        _viewMdoel.CountryLanguageID = CachedQueriedData.GetDefaultLanguage().CountryLanguageID;
        //    } else if (languages.Count > 1) {
        //        //it should be selected inside the register panel.
        //        ErrorCollector.AddMedium("You forgot you set your language.");
        //        return false;
        //    } else if (languages.Count == 1) {
        //        _viewMdoel.CountryLanguageID = languages[0].CountryLanguageID;
        //    }

        //    return true;
        //}


        //public bool TimezoneValidate() {
        //    var timezones = CachedQueriedData.GetTimezones(_viewMdoel.CountryID, 0);
        //    if (timezones != null && timezones.Count > 1) {
        //        //it should be selected inside the register panel.
        //        ErrorCollector.AddMedium("You forgot you set your time zone.");
        //        return false;
        //    } else if (timezones != null && timezones.Count == 1) {
        //        _viewMdoel.UserTimeZoneID = timezones[0].UserTimeZoneID;
        //        return true;
        //    } else {
        //        ErrorCollector.AddMedium(
        //            "You time zone not found. Please contact with admin and notify him/her about the issue to notify developer.");
        //    }
        //    return false;
        //}
        #region Overrides of Validator

        /// <summary>
        /// In this method all the  
        /// validation methods 
        /// should be added to the collection via AddValidation() method.
        /// </summary>
        public override void CollectValidation() {
            AddValidation(RegisterCodeValidate);
            AddValidation(IsUserDoesntExist);
            AddValidation(IsEmailDoesntExist);
            //AddValidation(LanguageValidate);
            //AddValidation(TimezoneValidate);
        }

        #endregion
    }
}