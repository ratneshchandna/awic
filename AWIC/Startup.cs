using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AWIC.Startup))]
namespace AWIC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
