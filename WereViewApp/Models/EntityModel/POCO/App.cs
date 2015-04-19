#region using block

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using WereViewApp.Models.EntityModel.Derivables;
using WereViewApp.Models.ViewModels;

#endregion

namespace WereViewApp.Models.EntityModel {
    public class App : AppSavingTextFields, IApp {
        public App() {
            FeaturedImages = new HashSet<FeaturedImage>();
            Reports = new HashSet<Report>();
            Reviews = new HashSet<Review>();
            TagAppRelations = new HashSet<TagAppRelation>();
            Galleries = new HashSet<HttpPostedFileBase>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long AppID { get; set; }

        public long PostedByUserID { get; set; }
        public short ReviewsCount { get; set; }
        public bool IsVideoExist { get; set; }
        public bool IsBlocked { get; set; }

        [Display(Name = "I agree to the terms and condition and publish my app.")]
        [Required]
        public bool IsPublished { get; set; }

        [Display(Name = "Platform Version", Description = "Important for users for better understanding of your app.")]
        [Required]
        public double PlatformVersion { get; set; }

        [Display(Name = "Viewed", Description = "Number of times viewed by users.")]
        public long TotalViewed { get; set; }

        public long WebsiteClicked { get; set; }
        public long StoreClicked { get; set; }
        public double AvgRating { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        [StringLength(70)]
        public string UrlWithoutEscapseSequence { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<FeaturedImage> FeaturedImages { get; set; }
        public virtual Platform Platform { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<TagAppRelation> TagAppRelations { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Title", Description ="(ASCII 50 chars) Title gives unique URLs which includes platform version, platform, category. Please specify those correctly to get a unique name. Eg. Plant vs. Zombies v2")]
        public string AppName { get; set; }

        [Display(Name = "Platform", Description = "Eg. Like iOS Platform version 7, Windows platform version 8.1 so on.")]
        [Range(0, 3000, ErrorMessage = "Sorry you have to been the range of 0-3000")]
        [Required]
        public byte PlatformID { get; set; }

        [Display(Name = "Category", Description = "Please choose the right category for your app.")]
        [Required]
        public short CategoryID { get; set; }

        [Display(Name = "Description",Description ="(Unicode 2000 chars) Describe your app fully the much you describe the much its optimized and rank. Try to include tags and app name to make it more SEO friendly.")]
        [StringLength(2000)]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Embed Video Link",Description ="Embed video link from youtube or anywhere. An embed video can boost your audiences. Even-though it's optional however we recommend to add a video.")]
        [StringLength(255)]
        public string YoutubeEmbedLink { get; set; }

        [Display(Name = "App Website", Description = "If you don't have any website then you can keep it blank.")]
        [StringLength(255)]
        public string WebSiteURL { get; set; }

        [Display(Name = "App Store",Description ="Please be relevant with your store URL because it help you get up to speed in sales and get in touch with more audenices..")]
        [StringLength(255)]
        public string StoreURL { get; set; }

        [StringLength(70)]
        public string URL { get; set; }

        #region Virtual Propertise

        [Display(Name = "Youtube video cover",Description = "[Single image 1MB] Resolution 481x440, however whatever you upload will be resized.")]
        public IEnumerable<HttpPostedFileBase> YoutubeCoverImage { get; set; }

        [Display(Name = "Gallery Images", Description = "[Max number 6 each 1MB] Resolution 1140x400, however whatever you upload will be resized.")]
        public IEnumerable<HttpPostedFileBase> Galleries { get; set; }

        [Display(Name = "Home Page (Featured)",Description ="[Single image 1MB] Resolution 1900 × 400, however whatever you upload will be resized. Better if you use png for this purpose.")]
        public HttpPostedFileBase HomePageFeatured { get; set; }

        [Display(Name = "Front Page Icon",Description = "[Single image 1MB] Resolution 122x115, however whatever you upload will be resized.")]
        public HttpPostedFileBase HomePageIcon { get; set; }

        [Display(Name = "Search Icon",Description = "[Single image 1MB] Resolution 117x177, however whatever you upload will be resized.")]
        public HttpPostedFileBase SearchIcon { get; set; }

        [Display(Name = "Suggestion Icon", Description = "[Single image 1MB] Best resolution 192x119, however whatever you upload will be resized.")]
        public HttpPostedFileBase SuggestionIcon { get; set; }

        #endregion

        #region Virtual Fields Add Not Saving anywhere

        public string HomeFeaturedBigImageLocation { get; set; }
        public string SuggestionIconLocation { get; set; }
        public string SearchIconLocation { get; set; }
        public string HomePageIconLocation { get; set; }
        public string YoutubeCoverImageLocation { get; set; }
        public List<DisplayGalleryImages> AppDetailsGalleryImages { get; set; }

        /// <summary>
        ///     Only will be set from extension method of App. GetAppUrl()
        /// </summary>
        public string AbsUrl { get; set; }

        /// <summary>
        ///     if false then load review.
        /// </summary>
        public bool IsReviewLoaded { get; set; }

        public byte? CurrentUserRatedAppValue { get; set; }

        #endregion
    }
}