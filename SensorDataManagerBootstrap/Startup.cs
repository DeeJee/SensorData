using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SensorDataManagerBootstrap.Startup))]
namespace SensorDataManagerBootstrap
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
