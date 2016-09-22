using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                defaults: new { controller = "SprintBacklog", action = "Index", id = UrlParameter.Optional }, namespaces: new string[] {
                    "StartIdea.UI.Controllers",
                    "StartIdea.UI.Areas.TeamMember",
                    "StartIdea.UI.Areas.ProductOwner"
                }
            );
        }
    }
}
