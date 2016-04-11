using System.Data.Entity.Migrations;
using WeReviewApp.Models.Context;

namespace WeReviewApp.Models.Migrations.DevIdentity {
    internal sealed class Configuration : DbMigrationsConfiguration<DevIdentityDbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Models\Migrations\DevIdentity";
        }

        protected override void Seed(DevIdentityDbContext context) {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
