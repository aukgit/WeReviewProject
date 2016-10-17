using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("ReviewApps", typeof(ReviewApps.Startup))]
namespace ReviewApps {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
