using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeReviewApp.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
        public Category()
        {
            this.Apps = new HashSet<App>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short CategoryID { get; set; }
        [Display(Name = "Category", Description = "Mobile application category name.")]

        public string CategoryName { get; set; }
        [Display(Name="Slug", Description="/App/Category/slug-name to display the category apps")]
        public string Slug { get; set; }
    
        public virtual ICollection<App> Apps { get; set; }
    }
}
