using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GladosNotificationService.Startup))]

namespace GladosNotificationService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}