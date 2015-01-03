using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WereViewApp.Startup))]
namespace WereViewApp {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
