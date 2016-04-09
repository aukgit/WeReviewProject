using System.Data.Entity.Core.Objects;

namespace WeReviewApp.Models.EntityModel {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using WeReviewApp.Modules.Extensions.Context;

    public partial class WereViewAppEntities : DevDbContext {
        public WereViewAppEntities()
            : base("name=WereViewAppEntities") {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            throw new UnintentionalCodeFirstException();
        }
        public virtual DbSet<App> Apps { get; set; }
        public virtual DbSet<AppDraft> AppDrafts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CellPhone> CellPhones { get; set; }
        public virtual DbSet<FeaturedImage> FeaturedImages { get; set; }
        public virtual DbSet<Gallery> Galleries { get; set; }
        public virtual DbSet<GalleryCategory> GalleryCategories { get; set; }
        public virtual DbSet<LatestSeenNotification> LatestSeenNotifications { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
		public virtual DbSet<MessageSeen> MessageSeens { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewLikeDislike> ReviewLikeDislikes { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagAppRelation> TagAppRelations { get; set; }
		public virtual DbSet<TempUpload> TempUploads { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPoint> UserPoints { get; set; }
        public virtual DbSet<UserPointSetting> UserPointSettings { get; set; }
        
        public virtual ObjectResult<AppsSearchResult> AppsSearch(string searchText)
        {
            var searchTextParameter = searchText != null ?
                new ObjectParameter("SearchText", searchText) :
                new ObjectParameter("SearchText", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AppsSearchResult>("AppsSearch", searchTextParameter);
        }
    
        public virtual int ResetAppDrafts()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ResetAppDrafts");
        }
    
        public virtual int ResetApps()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ResetApps");
        }
    
        public virtual int ResetAppsAndUsers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ResetAppsAndUsers");
        }
    
        public virtual int ResetReviews()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ResetReviews");
        }
    
        public virtual int ResetWholeSystem()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ResetWholeSystem");
        }
    }
}
