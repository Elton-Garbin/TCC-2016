using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model;
using StartIdea.UI.Areas.Admin.Models;
using StartIdea.UI.Areas.Admin.ViewModels;
using StartIdea.UI.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.Admin.Controllers
{
    public class UsuarioController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public UsuarioController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(int? PaginaGrid)
        {
            var usuarioVM = new UsuarioVM()
            {
                PaginaGrid = (PaginaGrid ?? 1)
            };

            usuarioVM.UsuarioList = GetGridDataSource(usuarioVM);

            return View(usuarioVM);
        }

        public ActionResult Create()
        {
            return View(new UsuarioVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioVM usuarioVM)
        {
            if (ModelState.IsValid)
            {
                var token = Guid.NewGuid();
                var usuario = new Usuario()
                {
                    CPF = usuarioVM.CPF,
                    Email = usuarioVM.Email,
                    UserName = usuarioVM.UserName,
                    IsActive = usuarioVM.IsActive,
                    Senha = Utils.Decrypt("@bc123ASD"),
                    TokenActivation = token
                };

                _dbContext.Usuarios.Add(usuario);
                _dbContext.SaveChanges();

                string link = "<a href='" + Url.Action("ResetPassword", "Authentication", new { area = "", token = token }, "http") + "'>Trocar Senha</a>";
                string body = string.Format(@"<p>Olá <b>{0}</b>,</p><br>
                                              <p>Você foi cadastrado no dia <b>{1}</b>.</p>
                                              <p>Por favor, clique no link para concluir o processo: {2}</p><br><p>Obrigado!</p>",
                                              usuario.UserName, DateTime.Now.ToShortDateString(), link);

                EmailService.EnviarEmail("Redefinição de Senha (StartIdea)", body, usuario.Email);

                return RedirectToAction("Index");
            }

            return View(usuarioVM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Usuario usuario = _dbContext.Usuarios.Find(id);
            if (usuario == null)
                return HttpNotFound();

            var usuarioVM = new UsuarioVM()
            {
                Id = usuario.Id,
                Email = usuario.Email,
                UserName = usuario.UserName,
                CPF = usuario.CPF,
                IsActive = usuario.IsActive
            };

            return View(usuarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioVM usuarioVM)
        {
            if (ModelState.IsValid)
            {
                var token = Guid.NewGuid();
                var usuario = _dbContext.Usuarios.Find(usuarioVM.Id);
                usuario.CPF = usuarioVM.CPF;
                usuario.Email = usuarioVM.Email;
                usuario.UserName = usuarioVM.UserName;
                usuario.DataAlteracao = DateTime.Now;

                bool enviaEmail = false;
                if (!usuario.IsActive && usuario.IsActive != usuarioVM.IsActive)
                {
                    enviaEmail = true;
                    usuario.TokenActivation = token;
                }

                usuario.IsActive = usuarioVM.IsActive;

                _dbContext.Entry(usuario).State = EntityState.Modified;
                _dbContext.SaveChanges();

                if (enviaEmail)
                {
                    string link = "<a href='" + Url.Action("ResetPassword", "Authentication", new { area = "", token = token }, "http") + "'>Trocar Senha</a>";
                    string body = string.Format(@"<p>Olá <b>{0}</b>,</p><br>
                                                  <p>Você foi reativado no dia <b>{1}</b>.</p>
                                                  <p>Por favor, clique no link para concluir o processo: {2}</p><br><p>Obrigado!</p>",
                                                  usuario.UserName, DateTime.Now.ToShortDateString(), link);

                    EmailService.EnviarEmail("Redefinição de Senha (StartIdea)", body, usuario.Email);
                }

                return RedirectToAction("Index");
            }

            return View(usuarioVM);
        }

        public ActionResult Perfil(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var perfilVM = new PerfilVM((int)id)
            {
                ProductOwner = GetProductOwner(),
                ScrumMaster = GetScrumMaster()
            };

            return View(perfilVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Perfil(PerfilVM perfilVM)
        {
            if (!ModelState.IsValid)
                return View("Perfil", perfilVM);

            if (perfilVM.Papel.Equals(TimeScrum.TimeDesenvolvimento) && string.IsNullOrEmpty(perfilVM.Descricao))
            {
                ModelState.AddModelError("Descricao", "Campo Função obrigatório.");
                return View("Perfil", perfilVM);
            }

            return View("Perfil", perfilVM);
        }

        private IPagedList<Usuario> GetGridDataSource(UsuarioVM usuarioVM)
        {
            return _dbContext.Usuarios.OrderBy(u => u.IsActive).ToPagedList(Convert.ToInt32(usuarioVM.PaginaGrid), 7);
        }

        private string GetProductOwner()
        {
            var productOwner = _dbContext.ProductOwners.Include(p => p.Usuario)
                .Where(p => p.IsActive)
                .FirstOrDefault();

            if (productOwner == null)
                return string.Empty;

            return productOwner.Usuario.UserName;
        }

        private string GetScrumMaster()
        {
            var scrumMaster = _dbContext.ScrumMasters.Include(s => s.Usuario)
                .Where(s => s.IsActive)
                .FirstOrDefault();

            if (scrumMaster == null)
                return string.Empty;

            return scrumMaster.Usuario.UserName;
        }
    }
}