using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model;
using StartIdea.Model.TimeScrum;
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
                var validation = _dbContext.Usuarios.Where(u => u.Email.ToLower().Equals(usuarioVM.Email.ToLower()) 
                                                             || u.UserName.ToLower().Equals(usuarioVM.UserName.ToLower()))
                                                    .FirstOrDefault();

                if (validation != null)
                {
                    if (validation.Email.ToLower() == usuarioVM.Email.ToLower())
                    {
                        ModelState.AddModelError("Email", "Email já cadastrado para um usuário");
                        return View(usuarioVM);
                    }

                    if(validation.UserName.ToLower() == usuarioVM.UserName.ToLower())
                    {
                        ModelState.AddModelError("UserName", "Nome de usuário já cadastrado");
                        return View(usuarioVM);
                    }
                }

                var token = Guid.NewGuid();
                var usuario = new Usuario()
                {
                    CPF = usuarioVM.CPF.Replace(".", "").Replace("-", ""),
                    Email = usuarioVM.Email,
                    UserName = usuarioVM.UserName,
                    IsActive = usuarioVM.IsActive,
                    IsAdmin = usuarioVM.IsAdmin,
                    Senha = Utils.Encrypt("@bc123ASD"),
                    TokenActivation = token
                };

                _dbContext.Usuarios.Add(usuario);
                _dbContext.SaveChanges();

                bool enviouEmail = true;
                if (usuarioVM.IsActive)
                {
                    string link = "<a href='" + Url.Action("ResetPassword", "Authentication", new { area = "", token = token }, "http") + "'>Trocar Senha</a>";
                    string body = string.Format(@"<p>Olá <b>{0}</b>,</p><br>
                                                  <p>Você foi cadastrado no dia <b>{1}</b>.</p>
                                                  <p>Por favor, clique no link para concluir o processo: {2}</p><br><p>Obrigado!</p>",
                                                     usuario.UserName, DateTime.Now.ToShortDateString(), link);

                    enviouEmail = EmailService.EnviarEmail("Redefinição de Senha (StartIdea)", body, usuario.Email);
                }

                return RedirectToAction("Edit", new
                {
                    id = usuario.Id,
                    naoEnviouEmail = !enviouEmail
                });
            }

            return View(usuarioVM);
        }

        public ActionResult Edit(int? id,
                                 bool naoEnviouEmail = false)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Usuario usuario = _dbContext.Usuarios.Find(id);
            if (usuario == null)
                return HttpNotFound();

            var usuarioVM = new UsuarioVM()
            {
                NaoEnviouEmail = naoEnviouEmail,
                Id = usuario.Id,
                Email = usuario.Email,
                UserName = usuario.UserName,
                CPF = usuario.CPF,
                IsActive = usuario.IsActive,
                IsAdmin = usuario.IsAdmin
            };

            return View(usuarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioVM usuarioVM)
        {
            if (ModelState.IsValid)
            {
                var UsuariosValidation = _dbContext.Usuarios.Where(u => u.Email.ToLower().Equals(usuarioVM.Email.ToLower()) ||
                                                        u.UserName.ToLower().Equals(usuarioVM.UserName.ToLower()));

                if (UsuariosValidation != null && UsuariosValidation.AsEnumerable().Count() > 0)
                {
                    foreach (var usuarioItem in UsuariosValidation)
                    {
                        if (usuarioItem.Email.ToLower() == usuarioVM.Email.ToLower() && usuarioItem.Id != usuarioVM.Id)
                        {
                            ModelState.AddModelError("Email", "Email já cadastrado para um usuário");
                            return View(usuarioVM);
                        }

                        if (usuarioItem.UserName.ToLower() == usuarioVM.UserName.ToLower() && usuarioItem.Id != usuarioVM.Id)
                        {
                            ModelState.AddModelError("UserName", "Nome de usuário já cadastrado");
                            return View(usuarioVM);
                        }
                    }
                }

                var token = Guid.NewGuid();
                var usuario = _dbContext.Usuarios.Find(usuarioVM.Id);
                usuario.CPF = usuarioVM.CPF.Replace(".", "").Replace("-", "");
                usuario.Email = usuarioVM.Email;
                usuario.UserName = usuarioVM.UserName;
                usuario.DataAlteracao = DateTime.Now;

                bool enviaEmail = false;
                if (!usuario.IsActive && usuario.IsActive != usuarioVM.IsActive)
                {
                    enviaEmail = true;
                    usuario.TokenActivation = token;
                }
                else if (!usuarioVM.IsActive)
                {
                    InativarTodosPerfis(usuario.Id);
                }

                usuario.IsActive = usuarioVM.IsActive;
                usuario.IsAdmin = usuarioVM.IsAdmin;

                _dbContext.Entry(usuario).State = EntityState.Modified;
                _dbContext.SaveChanges();

                bool enviouEmail = true;
                if (enviaEmail)
                {
                    string link = "<a href='" + Url.Action("ResetPassword", "Authentication", new { area = "", token = token }, "http") + "'>Trocar Senha</a>";
                    string body = string.Format(@"<p>Olá <b>{0}</b>,</p><br>
                                                  <p>Você foi reativado no dia <b>{1}</b>.</p>
                                                  <p>Por favor, clique no link para concluir o processo: {2}</p><br><p>Obrigado!</p>",
                                                  usuario.UserName, DateTime.Now.ToShortDateString(), link);

                    enviouEmail = EmailService.EnviarEmail("Redefinição de Senha (StartIdea)", body, usuario.Email);
                }

                if (enviouEmail)
                    return RedirectToAction("Index");
                else
                    usuarioVM.NaoEnviouEmail = !enviouEmail;
            }

            return View(usuarioVM);
        }

        public ActionResult Perfil(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _Id = 0;
            var _Papel = TimeScrum.TimeDesenvolvimento;
            var _Descricao = string.Empty;

            #region Consultando Perfis do Usuário
            var membroTime = _dbContext.MembrosTime.Include(m => m.Usuario)
                                       .Where(m => m.Usuario.Id == id)
                                       .FirstOrDefault();

            if (membroTime != null)
            {
                _Descricao = membroTime.Funcao;
                if (membroTime.IsActive)
                    _Id = membroTime.Id;
            }

            var productOwner = _dbContext.ProductOwners.Include(m => m.Usuario)
                                         .Where(m => m.Usuario.Id == id)
                                         .FirstOrDefault();

            if (productOwner != null)
            {
                if (productOwner.IsActive)
                {
                    _Papel = TimeScrum.ProductOwner;
                    _Id = productOwner.Id;
                }
            }

            var scrumMaster = _dbContext.ScrumMasters.Include(m => m.Usuario)
                                        .Where(m => m.Usuario.Id == id)
                                        .FirstOrDefault();

            if (scrumMaster != null)
            {
                if (scrumMaster.IsActive)
                {
                    _Papel = TimeScrum.ScrumMaster;
                    _Id = scrumMaster.Id;
                }
            }
            #endregion

            var perfilVM = new PerfilVM()
            {
                UsuarioId = (int)id,
                ProductOwner = GetProductOwner(),
                ScrumMaster = GetScrumMaster(),
                Id = _Id,
                Papel = _Papel,
                Descricao = _Descricao
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

            InativarTodosPerfis(perfilVM.UsuarioId);

            #region Atualizando Perfil
            switch (perfilVM.Papel)
            {
                case TimeScrum.TimeDesenvolvimento:
                    var membroTime = _dbContext.MembrosTime.Include(m => m.Usuario)
                                               .Where(m => m.Usuario.Id == perfilVM.UsuarioId)
                                               .FirstOrDefault() ?? new MembroTime();

                    if (membroTime.Id == 0)
                    {
                        membroTime.UsuarioId = perfilVM.UsuarioId;
                        membroTime.Funcao = perfilVM.Descricao;
                        membroTime.TimeId = CurrentUser.TimeId;

                        _dbContext.MembrosTime.Add(membroTime);
                    }
                    else
                    {
                        membroTime.IsActive = true;
                        membroTime.DataManutencao = DateTime.Now;
                        membroTime.Funcao = perfilVM.Descricao;

                        _dbContext.Entry(membroTime).State = EntityState.Modified;
                    }
                    break;
                case TimeScrum.ProductOwner:
                    InativarProductOwner();

                    var productOwner = _dbContext.ProductOwners.Include(m => m.Usuario)
                                                 .Where(m => m.Usuario.Id == perfilVM.UsuarioId)
                                                 .FirstOrDefault() ?? new Model.TimeScrum.ProductOwner();

                    if (productOwner.Id == 0)
                    {
                        productOwner.UsuarioId = perfilVM.UsuarioId;

                        _dbContext.ProductOwners.Add(productOwner);
                    }
                    else
                    {
                        productOwner.IsActive = true;
                        productOwner.DataManutencao = DateTime.Now;

                        _dbContext.Entry(productOwner).State = EntityState.Modified;
                    }
                    break;
                case TimeScrum.ScrumMaster:
                    InativarScrumMaster();

                    var scrumMaster = _dbContext.ScrumMasters.Include(m => m.Usuario)
                                                .Where(m => m.Usuario.Id == perfilVM.UsuarioId)
                                                .FirstOrDefault() ?? new Model.TimeScrum.ScrumMaster();

                    if (scrumMaster.Id == 0)
                    {
                        scrumMaster.UsuarioId = perfilVM.UsuarioId;
                        scrumMaster.TimeId = CurrentUser.TimeId;

                        _dbContext.ScrumMasters.Add(scrumMaster);
                    }
                    else
                    {
                        scrumMaster.IsActive = true;
                        scrumMaster.DataManutencao = DateTime.Now;

                        _dbContext.Entry(scrumMaster).State = EntityState.Modified;
                    }
                    break;
            }
            _dbContext.SaveChanges();
            #endregion

            return RedirectToAction("Edit", new
            {
                id = perfilVM.UsuarioId
            });
        }

        private IPagedList<Usuario> GetGridDataSource(UsuarioVM usuarioVM)
        {
            return _dbContext.Usuarios.Where(u => u.Id != 1)
                                      .OrderBy(u => u.IsActive)
                                      .ToPagedList(Convert.ToInt32(usuarioVM.PaginaGrid), 10);
        }

        private string GetProductOwner()
        {
            var productOwner = _dbContext.ProductOwners.Include(p => p.Usuario)
                                         .Where(p => p.IsActive)
                                         .FirstOrDefault();

            if (productOwner == null)
                return "Nenhum";

            return productOwner.Usuario.UserName;
        }

        private string GetScrumMaster()
        {
            var scrumMaster = _dbContext.ScrumMasters.Include(s => s.Usuario)
                                        .Where(s => s.IsActive)
                                        .FirstOrDefault();

            if (scrumMaster == null)
                return "Nenhum";

            return scrumMaster.Usuario.UserName;
        }

        private void InativarScrumMaster()
        {
            var scrumMaster = _dbContext.ScrumMasters
                                        .Where(s => s.IsActive)
                                        .FirstOrDefault();

            if (scrumMaster != null)
            {
                scrumMaster.IsActive = false;
                scrumMaster.DataManutencao = DateTime.Now;

                _dbContext.Entry(scrumMaster).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }

        private void InativarProductOwner()
        {
            var productOwner = _dbContext.ProductOwners
                                         .Where(s => s.IsActive)
                                         .FirstOrDefault();

            if (productOwner != null)
            {
                productOwner.IsActive = false;
                productOwner.DataManutencao = DateTime.Now;

                _dbContext.Entry(productOwner).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }

        private void InativarTodosPerfis(int UsuarioId)
        {
            #region TeamMember
            var membroTime = _dbContext.MembrosTime.Include(m => m.Usuario)
                                       .Where(m => m.Usuario.Id == UsuarioId)
                                       .FirstOrDefault();

            if (membroTime != null)
            {
                if (membroTime.IsActive)
                {
                    membroTime.IsActive = false;
                    membroTime.DataManutencao = DateTime.Now;

                    _dbContext.Entry(membroTime).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
            }
            #endregion

            #region ProductOwner
            var productOwner = _dbContext.ProductOwners.Include(m => m.Usuario)
                                         .Where(m => m.Usuario.Id == UsuarioId)
                                         .FirstOrDefault();

            if (productOwner != null)
            {
                if (productOwner.IsActive)
                {
                    productOwner.IsActive = false;
                    productOwner.DataManutencao = DateTime.Now;

                    _dbContext.Entry(productOwner).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
            }
            #endregion

            #region ScrumMaster
            var scrumMaster = _dbContext.ScrumMasters.Include(m => m.Usuario)
                                        .Where(m => m.Usuario.Id == UsuarioId)
                                        .FirstOrDefault();

            if (scrumMaster != null)
            {
                if (scrumMaster.IsActive)
                {
                    scrumMaster.IsActive = false;
                    scrumMaster.DataManutencao = DateTime.Now;

                    _dbContext.Entry(scrumMaster).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
            }
            #endregion
        }
    }
}