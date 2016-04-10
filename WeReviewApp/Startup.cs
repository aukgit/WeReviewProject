using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeReviewApp.Startup))]
namespace WeReviewApp {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
