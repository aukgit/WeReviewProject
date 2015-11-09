using System;
using System.Data.Entity;
using WereViewApp.Models.POCO.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WereViewApp.Models.POCO.IdentityCustomization;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace WereViewApp.Models.Context {

    #region Application DbContext

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim> {

        #region Required Part
        public ApplicationDbContext()
            : base("DefaultConnection") {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder
            //    .Properties()
            //    .Where(n => n.Name.ToLower() == "id")
            //    .Configure(p => p.IsKey());

            //self-reference 
            //modelBuilder.Entity<NavigationItem>()
            //       .HasOptional(i => i.ParentNavigation)
            //       .WithMany()
            //       .HasForeignKey(i => i.ParentNavigationID);

            modelBuilder.Properties<string>()
            .Where(x => x.Name == "Name")
            .Configure(c => c.HasMaxLength(25));
      

            modelBuilder.Properties<DateTime>()
            .Configure(c => c.HasColumnType("date"));

            modelBuilder.Entity<ApplicationRole>()
                .Property(n => n.Name)
                .IsUnicode(false)
                .HasMaxLength(50);

            modelBuilder.Entity<ApplicationUser>()
                .Property(n => n.Email)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(n => n.Id)
                .HasColumnName("UserID");
                //.HasColumnAnnotation("Index",new IndexAnnotation(new IndexAttribute("ix_primary_key_index_of_users") { IsClustered = true}));

            modelBuilder.Entity<ApplicationUser>()
                .Ignore(n => n.UserID);

            modelBuilder.Entity<ApplicationUser>()
                .Property(n => n.UserName)
                .IsUnicode(false)
                .HasMaxLength(30);

            modelBuilder.Entity<ApplicationUser>()
                .Property(n => n.PasswordHash)
                .IsUnicode(false)
                .HasMaxLength(70);

            modelBuilder.Entity<ApplicationUser>()
                .Property(n => n.PhoneNumber)
                .IsUnicode(false)
                .HasMaxLength(30);

            modelBuilder.Entity<ApplicationUser>()
                .Property(n => n.SecurityStamp)
                .IsUnicode(false)
                .HasMaxLength(38);

        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
        #endregion

        #region Additional tables
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryAlternativeName> CountryAlternativeNames { get; set; }
        public DbSet<CountryCurrency> CountryCurrencies { get; set; }
        public DbSet<CountryLanguage> CountryLanguages { get; set; }
        public DbSet<CountryLanguageRelation> CountryLanguageRelations { get; set; }
        //public DbSet<CountryBorder> CountryBorders { get; set; }
        public DbSet<CountryDetectByIP> CountryDetectByIPs { get; set; }
        public DbSet<CountryDomain> CountryDomains { get; set; }
        public DbSet<CountryTimezoneRelation> CountryTimezoneRelations { get; set; }
        public DbSet<CountryTranslation> CountryTranslations { get; set; }

        public DbSet<UserTimeZone> UserTimeZones { get; set; }
        public DbSet<RegisterCodeUserRelation> RegisterCodeUserRelations { get; set; }
        public DbSet<RegisterCode> RegisterCodes { get; set; }
        public DbSet<Navigation> Navigations { get; set; }
        public DbSet<NavigationItem> NavigationItems { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackCategory> FeedbackCategories { get; set; }
        public DbSet<TempUserRoleRelation> TempUserRoleRelations { get; set; }


        #endregion

        #region Save Override Methods
        /// <summary>
        /// Save changes and sends an email to the developer if any error occurred.
        /// </summary>
        /// <returns>>=0 :executed correctly. -1: error occurred.</returns>
        public override int SaveChanges() {
            try {
                return base.SaveChanges();

            } catch (Exception ex) {
                //async email
                AppVar.Mailer.HandleError(ex, "SaveChanges", "Error SaveChanges()");
                return -1;
            }

        }

        /// <summary>
        /// Save changes and sends an email to the developer if any error occurred.
        /// </summary>
        /// <returns>>=0 :executed correctly. -1: error occurred.</returns>
        public int SaveChanges(string methodName) {
            try {
                return base.SaveChanges();

            } catch (Exception ex) {
                //async email
                AppVar.Mailer.HandleError(ex, "SaveChanges - " + methodName, "Error SaveChanges()");
                return -1;
            }

        }

        /// <summary>
        /// Save changes and sends an email to the developer if any error occurred.
        /// </summary>
        /// <param name="entity">A single entity while saving if any error occurred send the info to the developer as well.</param>
        /// <returns>>=0 :executed correctly. -1: error occurred.</returns>
        public int SaveChanges(object entity) {
            try {
                return base.SaveChanges();
            } catch (Exception ex) {
                //async email
                AppVar.Mailer.HandleError(ex, "SaveChanges", "Error SaveChanges()", entity);
                return -1;
            }
        }

        /// <summary>
        /// Save changes and sends an email to the developer if any error occurred.
        /// </summary>
        /// <param name="entity">A single entity while saving if any error occurred send the info to the developer as well.</param>
        /// <returns>>=0 :executed correctly. -1: error occurred.</returns>
        public int SaveChanges(string methodName, object entity) {
            try {
                return base.SaveChanges();
            } catch (Exception ex) {
                //async email
                AppVar.Mailer.HandleError(ex, "SaveChanges - " + methodName, "Error SaveChanges()", entity);
                return -1;
            }
        }
        #endregion


    }

    #endregion

}