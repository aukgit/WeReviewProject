using System;
using System.Linq;
using DevMvcComponent.Error;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.POCO.Identity;
using WeReviewApp.Models.ViewModels;
using WeReviewApp.Modules.DevUser;
using WeReviewApp.Modules.UserError;

namespace WeReviewApp.Modules.Validations {
    public class DevUserValidator : Validator {
        private readonly RegisterViewModel _viewMdoel;
        private readonly ApplicationDbContext db;

        public DevUserValidator(RegisterViewModel viewMdoel, ErrorCollector errorCollector,
            ApplicationDbContext dbContext)
            : base(errorCollector) {
            _viewMdoel = viewMdoel;
            db = dbContext;
        }

        /// <summary>
        ///     Validate register code.
        ///     Returns true means validation is correct.
        /// </summary>
        /// <returns></returns>
        public bool RegisterCodeValidate() {
            if (!AppVar.Setting.IsRegisterCodeRequiredToRegister) {
                _viewMdoel.RegistraterCode = Guid.NewGuid();
                _viewMdoel.Role = -1;
            } else {
                var regCode =
                    db.RegisterCodes.FirstOrDefault(
                        n =>
                            n.IsUsed == false && n.RoleID == _viewMdoel.Role &&
                            n.RegisterCodeID == _viewMdoel.RegistraterCode && !n.IsExpired);
                if (regCode != null) {
                    if (regCode.ValidityTill <= DateTime.Now) {
                        // not valid
                        regCode.IsExpired = true;
                        ErrorCollector.AddMedium(MessageConstants.RegistercCodeExpired, "", "", "",
                            MessageConstants.SolutionContactAdmin);
                        db.SaveChanges();
                        return false;
                    }
                } else {
                    ErrorCollector.AddMedium(MessageConstants.RegistercCodeNotValid, "", "", "",
                        MessageConstants.SolutionContactAdmin);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Is user already exist.
        /// </summary>
        /// <returns></returns>
        public bool IsUserDoesntExist() {
            if (AppVar.Setting.IsFirstUserFound) {
                // first user is already registered.
                ApplicationUser user;
                if (UserManager.IsUserNameExistWithValidation(_viewMdoel.UserName, out user)) {
                    ErrorCollector.AddHigh(MessageConstants.UserNameExist, "", "", "",
                        MessageConstants.SolutionContactAdmin);
                    return false; // not valid
                }
            }
            return true;
        }

        /// <summary>
        ///     Is email already exist.
        /// </summary>
        /// <returns></returns>
        public bool IsEmailDoesntExist() {
            if (AppVar.Setting.IsFirstUserFound) {
                // first user is already registered.
                ApplicationUser user;
                if (UserManager.IsEmailExist(_viewMdoel.Email)) {
                    ErrorCollector.AddHigh(MessageConstants.EmailExist, "", "", "",
                        MessageConstants.SolutionContactAdmin);
                    return false; // not valid
                }
            }
            return true;
        }

        /// <summary>
        ///     Validate Language
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///     In this method all the
        ///     validation methods
        ///     should be added to the collection via AddValidation() method.
        /// </summary>
        public override void CollectValidation() {
            //AddValidation(RegisterCodeValidate);
            AddValidation(IsUserDoesntExist);
            AddValidation(IsEmailDoesntExist);
            //AddValidation(LanguageValidate);
            //AddValidation(TimezoneValidate);
        }

    }
}