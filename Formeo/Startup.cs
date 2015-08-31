using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Formeo.Startup))]
namespace Formeo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
