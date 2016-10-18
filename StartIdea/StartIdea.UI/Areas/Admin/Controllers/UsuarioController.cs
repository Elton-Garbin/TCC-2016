using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model;
using StartIdea.UI.Areas.Admin.Models;
using StartIdea.UI.Areas.Admin.ViewModels;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;
using StartIdea.UI.Models;

namespace StartIdea.UI.Areas.Admin.Controllers
{
    public class UsuarioController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public UsuarioController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(int? PaginaGrid,
                                  int? IdEdit,
                                  string DisplayCreate)
        {
            var usuarioVM = new UsuarioVM()
            {
                PaginaGrid = (PaginaGrid ?? 1),
                DisplayCreate = DisplayCreate
            };

            if ((IdEdit ?? 0) > 0)
            {
                var usuario = _dbContext.Usuarios.Find(IdEdit);
                if (usuario == null)
                    return HttpNotFound();

                usuarioVM.Id = usuario.Id;
                usuarioVM.Email = usuario.Email;
                usuarioVM.UserName = usuario.UserName;
                usuarioVM.CPF = usuario.CPF;
                usuarioVM.IsActive = usuario.IsActive;
                usuarioVM.DisplayEdit = "show";
            }

            usuarioVM.UsuarioList = GetGridDataSource(usuarioVM);

            return View(usuarioVM);
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
                    Senha = "oQ/OOuYzfK7mvK55NZJClHVCQj41dr6t/ZUai3EAq7M=",
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

            return View("Index", usuarioVM);
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

                usuarioVM.DisplayEdit = string.Empty;
            }

            usuarioVM.UsuarioList = GetGridDataSource(usuarioVM);

            return View("Index", usuarioVM);
        }

        private IPagedList<Usuario> GetGridDataSource(UsuarioVM usuarioVM)
        {
            return _dbContext.Usuarios.OrderBy(u => u.IsActive).ToPagedList(Convert.ToInt32(usuarioVM.PaginaGrid), 7);
        }
    }
}