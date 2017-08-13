using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(telstraGladosService.Startup))]

namespace telstraGladosService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}