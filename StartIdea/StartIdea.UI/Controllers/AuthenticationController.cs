using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using StartIdea.DataAccess;
using StartIdea.UI.Models;
using StartIdea.UI.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private StartIdeaDBContext dbContext;

        public AuthenticationController(StartIdeaDBContext _dbContext)
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
                                   && u.IsActive);

            if (usuario != null)
            {
                if (Utils.Decrypt(usuario.Senha).Equals(vm.Senha))
                {
                    AppAuth app = new AppAuth(AuthenticationManager, usuario);
                    app.SignIn(vm.PermanecerConectado);

                    return RedirectToAction("RedirectLoggedUser", new { role = app.Role });
                }
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

                string link = "<a href='" + Url.Action("ResetPassword", "Authentication", new { token = token }, "http") + "'>Trocar Senha</a>";
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

        public ActionResult ResetPassword(string token)
        {
            if (token == null)
                return RedirectToAction("Login");

            var usuario = dbContext.Usuarios.SingleOrDefault(u => u.TokenActivation.ToString() == token
                                                               && u.IsActive);

            if (usuario == null)
                return RedirectToAction("Login");

            var vm = new ResetPasswordVM()
            {
                Email = usuario.Email,
                TokenActivation = token
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var usuario = dbContext.Usuarios
                .Include(po => po.ProductOwners)
                .Include(sm => sm.ScrumMasters)
                .Include(mt => mt.MembrosTime)
                .SingleOrDefault(u => u.TokenActivation.ToString() == vm.TokenActivation
                                   && u.IsActive);

            if (usuario == null)
                return RedirectToAction("Login");

            if (Utils.Decrypt(usuario.Senha) == vm.Senha)
            {
                ModelState.AddModelError("", "Nova senha não pode ser igual a senha anterior.");
                return View(vm);
            }

            usuario.Senha = Utils.Encrypt(vm.Senha);
            usuario.TokenActivation = new Guid?();
            dbContext.Entry(usuario).State = EntityState.Modified;
            dbContext.SaveChanges();

            AppAuth app = new AppAuth(AuthenticationManager, usuario);
            app.SignIn();

            return RedirectToAction("RedirectLoggedUser", new { role = app.Role });
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
            if (role.Equals("Admin"))
                return RedirectToAction("Index", "Usuario", new { area = "Admin" });
            else if (role.Equals("ProductOwner"))
                return RedirectToAction("Index", "ProductBacklog", new { area = "ProductOwner" });
            else if (role.Equals("ScrumMaster"))
                return RedirectToAction("Index", "Sprint", new { area = "ScrumMaster" });
            else if (role.Equals("TeamMember"))
                return RedirectToAction("Index", "ProductBacklog", new { area = "TeamMember" });

            return RedirectToAction("Logout");
        }
    }
}