using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            else
                base.HandleUnauthorizedRequest(filterContext);
        }
    }
}