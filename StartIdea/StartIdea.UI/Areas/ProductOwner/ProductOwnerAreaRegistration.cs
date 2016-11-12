using System.Web.Mvc;

namespace StartIdea.UI.Areas.ProductOwner
{
    public class ProductOwnerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ProductOwner";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ProductOwner_default",
                "ProductOwner/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "StartIdea.UI.Areas.ProductOwner.Controllers" }
            );
        }
    }
}