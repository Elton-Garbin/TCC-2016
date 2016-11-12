using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using StartIdea.Model;
using System.Linq;
using System.Security.Claims;

namespace StartIdea.UI.Models
{
    public class AppAuth
    {
        private IAuthenticationManager _AuthenticationManager;
        private readonly Usuario _Usuario;
        public string PerfilRole { get; private set; }

        public AppAuth(IAuthenticationManager AuthenticationManager,
                       Usuario user)
        {
            _AuthenticationManager = AuthenticationManager;
            _Usuario = user;
            PerfilRole = GetPerfilRole();
        }

        public void SignIn(bool RememberBrowser = false)
        {
            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, _Usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, _Usuario.UserName),
                new Claim(ClaimTypes.Email, _Usuario.Email),
                new Claim("PerfilId", GetPerfilId()),
                new Claim("TimeId", "1"),
                new Claim(ClaimTypes.Role, PerfilRole)
            }, DefaultAuthenticationTypes.ApplicationCookie);

            if (!PerfilRole.Equals("Admin") && _Usuario.IsAdmin)
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            _AuthenticationManager.SignIn(new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = RememberBrowser
            }, identity);
        }

        public void SignOut()
        {
            _AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        private string GetPerfilId()
        {
            if (!Utils.IsEmpty(_Usuario.ProductOwners) && _Usuario.ProductOwners.First().IsActive)
                return _Usuario.ProductOwners.Single().Id.ToString();
            if (!Utils.IsEmpty(_Usuario.ScrumMasters) && _Usuario.ScrumMasters.First().IsActive)
                return _Usuario.ScrumMasters.Single().Id.ToString();
            if (!Utils.IsEmpty(_Usuario.MembrosTime) && _Usuario.MembrosTime.First().IsActive)
                return _Usuario.MembrosTime.Single().Id.ToString();

            return "0";
        }

        private string GetPerfilRole()
        {
            if (!Utils.IsEmpty(_Usuario.ProductOwners) && _Usuario.ProductOwners.First().IsActive)
                return "ProductOwner";
            else if (!Utils.IsEmpty(_Usuario.ScrumMasters) && _Usuario.ScrumMasters.First().IsActive)
                return "ScrumMaster";
            else if (!Utils.IsEmpty(_Usuario.MembrosTime) && _Usuario.MembrosTime.First().IsActive)
                return "TeamMember";
            else if (_Usuario.IsAdmin)
                return "Admin";

            return "None";
        }
    }
}