using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GAFER.Startup))]
namespace GAFER
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
