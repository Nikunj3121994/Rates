using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rates.Startup))]
namespace Rates
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
