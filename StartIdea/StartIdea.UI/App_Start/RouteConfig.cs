using System.Web.Mvc;
using System.Web.Routing;

namespace StartIdea.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Authentication", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "StartIdea.UI.Controllers" }
            );
        }
    }
}
