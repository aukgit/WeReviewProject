//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WereViewApp.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class App
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public App()
        {
            this.FeaturedImages = new HashSet<FeaturedImage>();
            this.Reviews = new HashSet<Review>();
            this.TagAppRelations = new HashSet<TagAppRelation>();
        }
    
        public long AppID { get; set; }
        public string AppName { get; set; }
        public byte PlatformID { get; set; }
        public double PlatformVersion { get; set; }
        public short CategoryID { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public long PostedByUserID { get; set; }
        public short ReviewsCount { get; set; }
        public bool IsVideoExist { get; set; }
        public string YoutubeEmbedLink { get; set; }
        public string WebsiteUrl { get; set; }
        public string StoreUrl { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsPublished { get; set; }
        public System.Guid UploadGuid { get; set; }
        public long TotalViewed { get; set; }
        public long WebsiteClicked { get; set; }
        public long StoreClicked { get; set; }
        public double AvgRating { get; set; }
        public System.DateTime ReleaseDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string UrlWithoutEscapseSequence { get; set; }
        public bool IsMultipleVersion { get; set; }
        public string TagsDisplay { get; set; }
        public string SupportedOSVersions { get; set; }
        public Nullable<byte> AppOfferTypeID { get; set; }
        public Nullable<double> Price { get; set; }
    
        public virtual AppOfferType AppOfferType { get; set; }
        public virtual Category Category { get; set; }
        public virtual Platform Platform { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeaturedImage> FeaturedImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TagAppRelation> TagAppRelations { get; set; }
    }
}
