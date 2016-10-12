using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using StartIdea.DataAccess;
using StartIdea.UI.Models;
using StartIdea.UI.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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

                if (app.Role.Equals("ProductOwner"))
                    return RedirectToAction("Index", "ProductBacklog", new { area = "ProductOwner" });
                else if (app.Role.Equals("ScrumMaster"))
                    return RedirectToAction("Index", "Sprint", new { area = "ScrumMaster" });
                else if (app.Role.Equals("TeamMember"))
                    return RedirectToAction("Index", "ProductBacklog", new { area = "TeamMember" });
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
    }
}