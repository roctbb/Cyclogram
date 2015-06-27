using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cyclo.Startup))]
namespace Cyclo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
