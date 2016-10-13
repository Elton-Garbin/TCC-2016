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
using System;

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

        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var usuario = dbContext.Usuarios.SingleOrDefault(u => u.Email == vm.Email);
            if (usuario != null)
            {
                var token = Guid.NewGuid();

                string link = "<a href='" + Url.Action("ResetPassword", "Account", new { token = token }, "http") + "'>Trocar Senha</a>";
                string body = string.Format(@"<p>Olá <b>{0}</b>,</p><br>
                                              <p>Você iniciou o processo de recuperação de senha no dia <b>{1}</b>.</p>
                                              <p>Por favor, clique no link para concluir o processo: {2}</p><br><p>Obrigado!</p>", 
                                              usuario.UserName, DateTime.Now.ToShortDateString(), link);

                EmailService.EnviarEmail("Recuperação de Senha (StartIdea)", body, usuario.Email);

                usuario.TokenActivation = token;
                dbContext.Entry(usuario).State = EntityState.Modified;
                dbContext.SaveChanges();
            }

            vm.CssClassMessage = "text-success";
            ModelState.AddModelError("", "Verifique o link enviado no e-mail informado!");
            return View(vm);
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