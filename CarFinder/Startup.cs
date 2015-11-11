using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarFinder.Startup))]
namespace CarFinder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
