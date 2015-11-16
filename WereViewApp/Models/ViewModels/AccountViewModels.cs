using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WereViewApp.Models.ViewModels {



    public class ExternalLoginListViewModel {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel {

        [Required(ErrorMessage = "User name is a required field.")]
        [Display(Name = "User name", Description = "(ASCII) User name to login. Must be unique. Character should be in between [aA-zZ], don't use space.")]
        [RegularExpression(@"^([A-Za-z]|[A-Za-z0-9_.]+)$", ErrorMessage = "Username shouldn't contain any space or punctuation or any alphanumeric character.")]
        [StringLength(30)]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First name is a required field.")]
        [Display(Name = "First Name", Description = "(ASCII) Character should be in between [aA-zZ], don't use space.")]
        [RegularExpression("^\\w+", ErrorMessage = "(ASCII)First name shouldn't contain any space or punctuation or any alphanumeric character or any number.")]
        [StringLength(15)]
        [MinLength(1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is a required field.")]
        [Display(Name = "Last Name", Description = "(ASCII) Character should be in between [aA-zZ], don't use space.")]
        [RegularExpression("^\\w+$", ErrorMessage = "(ASCII)Last name shouldn't contain any space or punctuation or any alphanumeric character or any number.")]
        [StringLength(15)]
        [MinLength(1)]
        public string LastName { get; set; }

        [Display(Name = "Role", Description = "Please select your specific role.")]
        public long Role { get; set; }
        [RegularExpression(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$", ErrorMessage = "Please give a valid GUID.")]
        [Display(Name = "Register Code", Description = "It should be a valid Guid. It is related to the roles.")]

        public Guid RegistraterCode { get; set; }

        //[Required(ErrorMessage = "Phone number is a required field.")]
        [Display(Name = "Phone", Description = "A valid phone number is required.")]
        [RegularExpression("^\\d+$", ErrorMessage = "Phone number should be only number digits(0-9).")]
        [StringLength(30)]
        [MinLength(5)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "(ASCII) Email address is a required field.")]
        [EmailAddress]
        [Display(Name = "Email Address", Description = "(ASCII)A valid email address is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "(ASCII) Password is a required field.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Description = "(ASCII)Minimum 6 digit, no other restriction.")]
        [MinLength(6)]

        public string Password { get; set; }

        [Required(ErrorMessage = "(ASCII) Confirm password is a required field.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password", Description = "Confirm password should match the password given above.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please select your verified age.")]
        [Display(Name = "Date of Birth", Description = "If you do not use verified date, our lawsuit could ban you.")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please select your country, it's not valid.")]
        [Display(Name = "Country", Description = "Please select your exact country, it's going to verify against your IP.")]
        public int CountryID { get; set; }

        //[Required(ErrorMessage = "Please select your language.")]
        //[Display(Name = "Language", Description = "Please select your language.")]
        public int CountryLanguageID { get; set; }
        public int UserTimeZoneID { get; set; }

    }

    public class ResetPasswordViewModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
