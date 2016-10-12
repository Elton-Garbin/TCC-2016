using Microsoft.Owin;
using Owin;
using StartIdea.UI;

[assembly: OwinStartup(typeof(Startup))]
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
