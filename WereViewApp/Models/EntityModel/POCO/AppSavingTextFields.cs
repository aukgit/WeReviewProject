using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WereViewApp.Models.EntityModel{
    [Serializable]
    public class AppSavingTextFields {
        #region Virtual
        [StringLength(250)]
        [Display(Name = "Idea by", Description = "(250 Characters) Use comma separator.")]

        public string IdeaBy { get; set; }
        [StringLength(250)]
        [Display(Name = "Developers Names", Description = "(250 Characters) Use comma separator.")]
        public string Developers { get; set; }
        [StringLength(250)]
        [Display(Name = "Publishers' Names", Description = "(250 Characters) Use comma separator.")]

        public string Publishers { get; set; }

        [Display(Name = "Tags", Description = "(Use comma separated values to save your tags. Highest 10 tags are accepted. Please be relevant with details and it make the app more SEO friendly.")]
        [StringLength(250)]
        [Required]
        public string Tags { get; set; } 
        #endregion

        [Required]
        public System.Guid UploadGuid { get; set; }
    }
}