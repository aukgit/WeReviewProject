using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeReviewApp.Models.POCO.Identity;

namespace WeReviewApp.Models.POCO.IdentityCustomization {
    public class Country {
        #region Columns

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryID { get; set; }

        [StringLength(50)]
        [Required]
        public string CountryName { get; set; }

        [StringLength(25)]
        public string Capital { get; set; }

        [Column(TypeName = "CHAR")]
        [MaxLength(2)]
        [Required]
        public string Alpha2Code { get; set; }

        [Column(TypeName = "CHAR")]
        [MaxLength(3)]
        [Required]
        public string Alpha3Code { get; set; }

        public int CallingCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string NationalityName { get; set; }

        /// <summary>
        ///     Like Asia
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Region { get; set; }

        /// <summary>
        ///     Like Southern Asia
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [StringLength(25)]
        public string SubRegion { get; set; }

        public long Population { get; set; }

        public float Area { get; set; }

        [StringLength(12)]
        public string Culture { get; set; }

        /// <summary>
        ///     https://en.wikipedia.org/wiki/Gini_coefficient
        ///     measure of statistical dispersion intended to represent the income distribution of a nation's residents
        /// </summary>
        public float GiniCoefficient { get; set; }

        /// <summary>
        ///     Ordering Relevance
        /// </summary>
        public float Relevance { get; set; }

        /// <summary>
        ///     Represent in map like this LatitudeStartingPoint,LatitudeEndingPoint
        /// </summary>
        public float LatitudeStartingPoint { get; set; }

        /// <summary>
        ///     Represent in map like this LatitudeStartingPoint,LatitudeEndingPoint
        /// </summary>
        public float LatitudeEndingPoint { get; set; }

        //public short? PagesAvailableOnGeolocationSite { get; set; }

        /// <summary>
        ///     TimezoneID non-relating from UserTimeZoneTable
        ///     Only those which doesn't have multiple timezones will be here.
        /// </summary>
        public int? RelatedTimeZoneID { get; set; }

        public bool IsSingleTimeZone { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        [Required]
        public string DisplayCountryName { get; set; }

        #endregion

        //[ForeignKey("CountryID")]
        //public ICollection<CountryBorder> CountryBordersCountryRelations { get; set; }

        #region Relations

        [ForeignKey("CountryID")]
        public ICollection<CountryAlternativeName> CountryAlternativeNames { get; set; }

        [ForeignKey("CountryID")]
        public ICollection<CountryCurrency> CountryCurrencies { get; set; }

        [ForeignKey("CountryID")]
        public ICollection<CountryDetectByIP> CountryDetectByIPs { get; set; }

        [ForeignKey("CountryID")]
        public ICollection<CountryTimezoneRelation> CountryTimezoneRelations { get; set; }

        [ForeignKey("CountryID")]
        public ICollection<CountryTranslation> CountryTranslations { get; set; }

        [ForeignKey("CountryID")]
        public ICollection<CountryLanguageRelation> CountryLanguageRelations { get; set; }

        [ForeignKey("CountryID")]
        public ICollection<CountryDomain> CountryDomains { get; set; }

        [ForeignKey("CountryID")]
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        #endregion
    }
}