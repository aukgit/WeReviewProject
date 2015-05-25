//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace WereViewApp.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
        public Category()
        {
            this.Apps = new HashSet<App>();
        }
    
        public short CategoryID { get; set; }
        [Display(Name = "Category", Description = "Mobile application category name.")]

        public string CategoryName { get; set; }
        [Display(Name="Slug", Description="/App/Category/slug-name to display the category apps")]
        public string Slug { get; set; }
    
        public virtual ICollection<App> Apps { get; set; }
    }
}
