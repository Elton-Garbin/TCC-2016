using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace StartIdea.UI
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Authentication/Login"),
                LogoutPath = new PathString("/Authentication/Logout"),
                CookieSecure = CookieSecureOption.SameAsRequest,
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(30)
            });
        }
    }
}