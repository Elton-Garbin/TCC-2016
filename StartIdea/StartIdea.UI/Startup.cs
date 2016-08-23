using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StartIdea.UI.Startup))]
namespace StartIdea.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
