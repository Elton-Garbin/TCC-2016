using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using StartIdea.DataAccess;
using StartIdea.UI.Models;
using StartIdea.UI.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Claims;

namespace StartIdea.UI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private StartIdeaDBContext dbContext;

        public AccountController(StartIdeaDBContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public ActionResult Login()
        {
            if (AuthenticationManager.User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)AuthenticationManager.User.Identity;
                string Role = identity.Claims.Where(c => c.Type == ClaimTypes.Role)
                                             .Select(c => c.Value).SingleOrDefault();

                return RedirectToAction("RedirectLoggedUser", new { role = Role });
            }            

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View();

            var usuario = dbContext.Usuarios
                .Include(po => po.ProductOwners)
                .Include(sm => sm.ScrumMasters)
                .Include(mt => mt.MembrosTime)
                .SingleOrDefault(u => u.Email == vm.Email 
                                   && u.Senha == vm.Senha);

            if (usuario != null)
            {
                AppAuth app = new AppAuth(AuthenticationManager, usuario);
                app.SignIn(vm.PermanecerConectado);

                return RedirectToAction("RedirectLoggedUser", new { role = app.Role });
            }

            ModelState.AddModelError("", "E-mail ou Senha inválido.");
            return View();
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public ActionResult RedirectLoggedUser(string role)
        {
            if (role.Equals("ProductOwner"))
                return RedirectToAction("Index", "ProductBacklog", new { area = "ProductOwner" });
            else if (role.Equals("ScrumMaster"))
                return RedirectToAction("Index", "Sprint", new { area = "ScrumMaster" });
            else if (role.Equals("TeamMember"))
                return RedirectToAction("Index", "ProductBacklog", new { area = "TeamMember" });

            return RedirectToAction("Logout");
        }
    }
}