using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StankinQuestionnaire.Startup))]
namespace StankinQuestionnaire
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
