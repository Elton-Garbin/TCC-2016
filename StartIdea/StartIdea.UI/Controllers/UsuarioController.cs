﻿using Microsoft.Owin.Security;
using StartIdea.DataAccess;
using StartIdea.UI.Models;
using StartIdea.UI.ViewModels;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    public class UsuarioController : Controller
    {
        private StartIdeaDBContext _dbContext;

        public UsuarioController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            var identity = (ClaimsIdentity)AuthenticationManager.User.Identity;
            string Id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                       .Select(c => c.Value).SingleOrDefault() ?? "0";

            var usuario = _dbContext.Usuarios.Find(Convert.ToInt32(Id));
            if (usuario == null)
                return HttpNotFound();

            var usuarioVM = new UsuarioVM();
            usuarioVM.Id = usuario.Id;
            if (usuario.Foto != null)
                usuarioVM.ImageBase64 = Convert.ToBase64String(usuario.Foto);

            return View(usuarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioVM usuarioVM)
        {
            if (!ModelState.IsValid)
                return View("Index", usuarioVM);

            var usuario = _dbContext.Usuarios.Find(usuarioVM.Id);
            if (usuario == null)
                return HttpNotFound();

            if (usuarioVM.ImageUpload != null && !(usuarioVM.ImageUpload.ContentType.Contains("image/png")))
            {
                ModelState.AddModelError("", "Apenas o formato *.png é suportado para a foto de perfil.");
                return View("Index", usuarioVM);
            }
            else if (usuarioVM.ImageUpload != null)
            {
                using (var reader = new BinaryReader(usuarioVM.ImageUpload.InputStream))
                {
                    usuario.Foto = reader.ReadBytes(usuarioVM.ImageUpload.ContentLength);
                }

                usuarioVM.ImageBase64 = Convert.ToBase64String(usuario.Foto);
            }

            if (usuarioVM.TrocarSenha)
            {
                if (usuarioVM.Senha != Utils.Decrypt(usuario.Senha))
                {
                    ModelState.AddModelError("", "Senha atual inválida.");
                    return View("Index", usuarioVM);
                }
                else if (usuarioVM.Senha == usuarioVM.NovaSenha)
                {
                    ModelState.AddModelError("", "Nova senha não pode ser igual a senha anterior.");
                    return View("Index", usuarioVM);
                }

                usuario.Senha = Utils.Encrypt(usuarioVM.NovaSenha);

                usuarioVM.CssClassMessage = "text-success";
                ModelState.AddModelError("", "Senha alterada com sucesso.");
            }

            usuario.TokenActivation = new Guid?();

            _dbContext.Entry(usuario).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return View("Index", usuarioVM);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
    }
}