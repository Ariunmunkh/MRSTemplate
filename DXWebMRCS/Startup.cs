using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DXWebMRCS.Startup))]
namespace DXWebMRCS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}