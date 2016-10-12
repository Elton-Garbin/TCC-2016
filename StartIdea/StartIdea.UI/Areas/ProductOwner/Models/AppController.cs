using StartIdea.UI.Models;
using System.Security.Claims;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ProductOwner.Models
{
    [CustomAuthorize(Roles = "ProductOwner")]
    public abstract class AppController : Controller
    {
        public AppUser CurrentUser
        {
            get { return new AppUser(User as ClaimsPrincipal); }
        }
    }
}