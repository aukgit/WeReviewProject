using System.ComponentModel.DataAnnotations;

namespace ReviewApps.Models.ViewModels {
    public class ForgotPasswordViewModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}