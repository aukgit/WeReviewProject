//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeReviewApp.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using WeReviewApp.Modules.Uploads;
    public partial class Gallery : IUploadableFile
    {
        public Guid GalleryID { get; set; }
        public Guid UploadGuid { get; set; }
        public int GalleryCategoryID { get; set; }
        public byte Sequence { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
		//Virtual field
        public HttpPostedFileBase File { get; set; }

        public string Extension { get; set; }

        public virtual GalleryCategory GalleryCategory { get; set; }
    }
}