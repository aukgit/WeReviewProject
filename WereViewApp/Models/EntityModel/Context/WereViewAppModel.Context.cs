﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WereViewApp.Models.EntityModel {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using WereViewApp.Modules.Extensions.Context;

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

        public DbSet<CellPhone> CellPhones { get; set; }
        public DbSet<FeaturedImage> FeaturedImages { get; set; }

        public virtual DbSet<Gallery> Galleries { get; set; }
        public virtual DbSet<GalleryCategory> GalleryCategories { get; set; }
        public virtual DbSet<LatestSeenNotification> LatestSeenNotifications { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewLikeDislike> ReviewLikeDislikes { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagAppRelation> TagAppRelations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPoint> UserPoints { get; set; }
        public virtual DbSet<UserPointSetting> UserPointSettings { get; set; }
        public virtual DbSet<MessageSeen> MessageSeens { get; set; }
        public virtual DbSet<TempUpload> TempUploads { get; set; }
    }
}
