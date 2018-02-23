using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bloggy.Startup))]
namespace Bloggy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
