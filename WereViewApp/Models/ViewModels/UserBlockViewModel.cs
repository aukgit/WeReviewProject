using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WereViewApp.Models.ViewModels {
    public class UserBlockViewModel {
        [Required]
        [Key]
        public long UserId { get; set; }

        public string UserName { get; set; }
        [Display(Name = "Say sorry in the email.")]
        public bool? SaySorry { get; set; }

        [StringLength(20)]
        [Display(Name = "Blocking Reason", Description = "Reason why he/she is blocked.")]
        public string Reason { get; set; }

    }
}