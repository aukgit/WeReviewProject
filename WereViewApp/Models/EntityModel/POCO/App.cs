#region using block

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using WeReviewApp.Models.EntityModel.Derivables;
using WeReviewApp.Models.ViewModels;

#endregion

namespace WeReviewApp.Models.EntityModel {
    public class App : AppSavingTextFields, IApp {
        public App() {
            FeaturedImages = new HashSet<FeaturedImage>();
            Reviews = new HashSet<Review>();
            TagAppRelations = new HashSet<TagAppRelation>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long AppID { get; set; }
        [StringLength(50)]
        [Required]
        [Display(Name = "Title", Description = "(ASCII 50 chars) Title gives unique URLs which includes platform version, platform, category. Please specify those correctly to get a unique name. Eg. Plant vs. Zombies v2")]
        public string AppName { get; set; }

        [Display(Name = "Platform", Description = "Eg. Like Apple Platform version 7, Windows platform version 8.1 so on.")]
        [Range(0, 3000, ErrorMessage = "Sorry you have to been the range of 0-3000")]
        [Required]
        public byte PlatformID { get; set; }

        [Display(Name = "Category", Description = "Please choose the right category for your app.")]
        [Required]
        public short CategoryID { get; set; }

        [Display(Name = "Description", Description = "(Unicode 2000 chars) Describe your app fully the much you describe the much its optimized and rank. Try to include tags and app name to make it more SEO friendly.")]
        [StringLength(2000)]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Embed Video Link", Description = "Embed video link from youtube or anywhere. An embed video can boost your audiences. Even-though it's optional however we recommend to add a video.")]
        [StringLength(255)]
        public string YoutubeEmbedLink { get; set; }

        [Display(Name = "App Website", Description = "If you don't have any website then you can keep it blank.")]
        [StringLength(255)]
        public string WebsiteUrl { get; set; }

        [Display(Name = "App Store", Description = "Please be relevant with your store URL because it help you get up to speed in sales and get in touch with more audenices..")]
        [StringLength(255)]
        public string StoreUrl { get; set; }
        public long PostedByUserID { get; set; }

        public bool IsVideoExist { get; set; }
        public bool IsBlocked { get; set; }

        [Display(Name = "I agree to the terms and condition and publish my app.")]
        [Required]
        public bool IsPublished { get; set; }

        [Display(Name = "Platform Version", Description = "Important for users for better understanding of your app.")]
        [Required]
        public double PlatformVersion { get; set; }
        // this field is required to upload gallery images and identify this app.
        //public Guid UploadGuid { get; set; }  coming from Super class

        [Display(Name = "Viewed", Description = "Number of times viewed by users.")]
        public long TotalViewed { get; set; }

        public long WebsiteClicked { get; set; }
        public long StoreClicked { get; set; }
        /// <summary>
        /// This will be updated by Logics.FixRatingInApp() method
        /// When any user rated this app. Static value field.
        /// </summary>
        public double AvgRating { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        [StringLength(70)]
        public string UrlWithoutEscapseSequence { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<FeaturedImage> FeaturedImages { get; set; }
        public virtual Platform Platform { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<TagAppRelation> TagAppRelations { get; set; }



        [StringLength(70)]
        public string Url { get; set; }

        #region Virtual Propertise

        [Display(Name = "Youtube video cover", Description = "[Single image 1MB] Resolution 1140x400, however whatever you upload will be resized.")]
        public HttpPostedFileBase YoutubeCoverImage { get; set; }

        [Display(Name = "Gallery Images", Description = "[Max number 6 each 1MB] Resolution 1140x400, however whatever you upload will be resized.")]
        public IEnumerable<HttpPostedFileBase> Galleries { get; set; }

        [Display(Name = "Home Page (Featured big)", Description = "[Single image 1MB] Resolution 481x440, however whatever you upload will be resized. Better if you use png for this purpose.")]
        public HttpPostedFileBase HomePageFeatured { get; set; }

        [Display(Name = "Home Page (Icon)", Description = "[Single image 1MB] Resolution 122x115, however whatever you upload will be resized.")]
        public HttpPostedFileBase HomePageIcon { get; set; }

        [Display(Name = "Search Icon", Description = "[Single image 1MB] Resolution 117x177, however whatever you upload will be resized.")]
        public HttpPostedFileBase SearchIcon { get; set; }

        [Display(Name = "Suggestion Icon", Description = "[Single image 1MB] Best resolution 192x119, however whatever you upload will be resized.")]
        public HttpPostedFileBase SuggestionIcon { get; set; }

        #endregion

        #region Virtual Fields Add Not Saving anywhere

        /**
         * app-details page indicates 
         * Controller : App
         * Action : SingleAppDisplay
         * URL: /Apps/Apple-8/Games/plant-vs-zombies
         * */

        /// <summary>
        /// Virtual Field : Home page gallery image location for this app
        /// </summary>
        public string HomeFeaturedBigImageLocation { get; set; }
        /// <summary>
        /// Virtual Field :
        /// Virtual Property not going to save.
        /// Used to get the location of App-details suggestion app icons
        /// </summary>
        public string SuggestionIconLocation { get; set; }
        /// <summary>
        /// Virtual Field :
        /// Virtual Property not going to save.
        /// Used to get the location of Search/Profile/Category page app icons
        /// </summary>
        public string SearchIconLocation { get; set; }
        /// <summary>
        /// Virtual Field :
        /// Virtual Property not going to save.
        /// Used to get the location of Home page app icons
        /// </summary>
        public string HomePageIconLocation { get; set; }
        /// <summary>
        /// Virtual Field :
        /// Virtual Property not going to save.
        /// Used to get the location of youtube cover image
        /// </summary>
        public string YoutubeCoverImageLocation { get; set; }
        /// <summary>
        /// Virtual Field :
        /// not going to save.
        /// It is use to display gallery images in app-details page
        /// </summary>
        public List<DisplayGalleryImages> AppDetailsGalleryImages { get; set; }

        /// <summary>
        /// Virtual Field : 
        /// Will be only set from extension method of App.GetAbsoluteUrl()
        /// </summary>
        public string AbsUrl { get; set; }

        /// <summary>
        /// Virtual Field : 
        /// if false then load review by force.
        /// Or else it will load from cache.
        /// </summary>
        public bool IsReviewAlreadyLoaded { get; set; }
        /// <summary>
        /// Virtual Field 
        /// </summary>
        public byte? CurrentUserRatedAppValue { get; set; }

        /// <summary>
        /// Virtual Field : 
        /// to only keep the review likes and dislikes
        /// </summary>
        public virtual List<ReviewLikeDislike> ReviewLikeDislikesCollection { get; set; }



        /// <summary>
        /// Virtual Field : 
        /// This property is loaded from 
        /// Logics.cs->LoadReviewIntoApp() method
        /// </summary>
        public short ReviewsCount { get; set; }


        /// <summary>
        /// Virtual Field : 
        /// How many reviews displaying in app-detail page.
        /// Logics.cs->LoadReviewIntoApp() method
        /// Only be updated if skip  = 0 from that method 
        /// otherwise update it from the partial controller
        /// </summary>
        public int? ReviewDisplayingCount { get; set; }
        #endregion
    }
}