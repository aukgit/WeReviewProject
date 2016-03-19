using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WereViewApp.Models.POCO.IdentityCustomization;
using WereViewApp.Modules.Extensions.Context;

namespace WereViewApp.Models.Context {
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