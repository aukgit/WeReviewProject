using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WeReviewApp.Models.POCO.IdentityCustomization;
using WeReviewApp.Modules.Extensions.Context;

namespace WeReviewApp.Models.Context {
    public class DevIdentityDbContext : DevDbContext {
        public DevIdentityDbContext()
            : base("name=DefaultConnection") {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<CoreSetting> CoreSettings { get; set; }

        public DbSet<ImageResizeSetting> ImageResizeSettings { get; set; }
    }
}