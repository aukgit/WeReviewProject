using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WereViewApp.Models.POCO.IdentityCustomization {
    public class CoreSetting {
        [DisplayName("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public byte CoreSettingID { get; set; }
        [DisplayName("Application Name")]
        [StringLength(100)]
        [Required]

        public string ApplicationName { get; set; }
        [StringLength(200)]
        [Required]
        [Display(Name = "Subtitle", Description = "Website's subtitle.")]
        public string ApplicationSubtitle { get; set; }
        [StringLength(300)]
        [Required]
        [Display(Name = "Description", Description = "Website's description.")]
        public string ApplicationDescription { get; set; }

        [StringLength(400)]
        [Required]
        public string Address { get; set; }

        [Display(Description = "Office phone number in only numeric format.")]
        [Required]
        public long OfficePhone { get; set; }
        [Display(Description = "Fax in only numeric format.")]
        [Required]
        public long Fax { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(256)]
        [Required]
        [DisplayName("Office Email")]
        public string OfficeEmail { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(256)]
        [DisplayName("Office Email")]
        [Required]
        public string SupportEmail { get; set; }
        [Display(Name = "Support Phone", Description = "Support phone number in numeric format.")]
        [Required]
        public long SupportPhone { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(256)]
        [DisplayName("Office Email")]
        [Required]
        public string MarketingEmail { get; set; }

        [Display(Name = "Marketing Phone", Description = "Support phone number in numeric format.")]
        [Required]
        public long MarketingPhone { get; set; }

        [StringLength(120)]
        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [Required]
        [StringLength(35)]
        public string Language { get; set; }

     

        [StringLength(20)]
        [Required]
        [DisplayName("Admin Location")]
        public string AdminLocation { get; set; }

        [StringLength(500)]
        [Required]
        [Display(Name = "Live Url", Description = "Your live site url (without slash).")]
        public string LiveUrl { get; set; }

        [StringLength(500)]
        [Required]
        [Display(Name = "Testing Url", Description = "Your testing environment url (without slash).")]
        public string TestingUrl { get; set; }
        [StringLength(500)]
        [Required]
        [Display(Name="Service Url", Description="Common Service Controller Url")]
        public string ServicesControllerUrl { get; set; }
        [StringLength(500)]
        [Required]
        [Display(Name = "Api Url", Description = "Common Api Controller Url")]
        public string ApiControllerUrl { get; set; }
        [StringLength(256)]
        [Required]
        [EmailAddress]
        [DisplayName("Admin Email")]
        public string AdminEmail { get; set; }
        [DisplayName("Admin Name")]
        [StringLength(60)]
        [Column(TypeName = "VARCHAR")]
        public string AdminName { get; set; }
        [StringLength(256)]
        [Required]
        [Display(Name = "Developer's Email", Description = "Developer's Email.")]
        [EmailAddress]
        public string DeveloperEmail { get; set; }
        [Required]
        [Display(Name = "Page Item Size", Description = "Number of items display in one page.")]
        public long PageItems { get; set; }

        [StringLength(256)]
        [Required]
        [Display(Name = "Email", Description = "Email address where all the emails will be sent.")]

        [EmailAddress]
        public string SenderEmail { get; set; }
        [Column(TypeName = "VARCHAR")]
        [Required]
        [StringLength(30)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Description = "Email password.")]
        public string SenderEmailPassword { get; set; }
        [StringLength(256)]
        [Required]
        [Display(Name="Sender Display", Description="Email sender display name.")]
        public string SenderDisplay { get; set; }
        [StringLength(45)]
        [Display(Name = "Webmaster Meta", Description = "meta name='google-site-verification' content='val'")]
        [Column(TypeName = "VARCHAR")]
        [Required]
        public string GoogleMetaTag { get; set; }

        [StringLength(25)]
        [Display(Name = "SMTP Host", Description = "Example: smtp.gmail.com")]
        [Column(TypeName = "VARCHAR")]
        public string SmtpHost { get; set; }

        [Display(Name = "SMTP Port", Description = "Example: smtp.gmail.com, for ssl enabled it's 587")]
        public int SmtpMailPort { get; set; }
        public long FacebookClientID { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(40)]
        public string FacebookSecret { get; set; }
        [Display(Name = "Gallery Max", Description = "App details page , max gallery image number to upload.")]

        public byte GalleryMaxPictures { get; set; }
        [Display(Name = "Max Draft", Description = "Apps max draft number.")]

        public byte MaxDraftPostByUsers { get; set; }
        //[DefaultValue(1)]
        //[Required]
        //[DisplayName("Steps in Registration")]
        //public byte RegistrationSteps { get; set; }
        /// <summary>
        /// If disable then it will not require a password to get in admin panel.
        /// </summary>
        [DisplayName("Is Authentication Required for Admin Panel.")]
        public bool IsAuthenticationEnabled { get; set; }

        [DisplayName("Is in testing mode.")]
        public bool IsInTestingEnvironment { get; set; }

        [DisplayName("Should registration code be linked with user id?")]
        public bool ShouldRegistrationCodeBeLinkedWithUser { get; set; }
        [DisplayName("Register Code never expires.")]
        public bool DoesRegisterCodeNeverExpires { get; set; }
        [DisplayName("Register Code required to register.")]
        public bool IsRegisterCodeRequiredToRegister { get; set; }
        [DisplayName("Is SSL Enabled for SMTP configuration.")]
        public bool IsSMTPSSL { get; set; }

        [DisplayName("Is Confirmation mail required.")]
        public bool IsConfirmMailRequired { get; set; }
        [DisplayName("Is Facebook authentication enabled.")]
        public bool IsFacebookAuthentication { get; set; }

        [DisplayName("On Error Notify Developer")]
        public bool NotifyDeveloperOnError { get; set; }

        [DisplayName("First User found")]
        public bool IsFirstUserFound { get; set; }

    }
}