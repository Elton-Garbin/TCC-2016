using System.Web.Mvc;

namespace StartIdea.UI.Areas.ScrumMaster
{
    public class ScrumMasterAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ScrumMaster";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ScrumMaster_default",
                "ScrumMaster/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}