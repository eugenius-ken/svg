using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(svg.Startup))]
namespace svg
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
