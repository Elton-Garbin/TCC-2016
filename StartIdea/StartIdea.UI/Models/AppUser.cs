using System;
using System.Security.Claims;

namespace StartIdea.UI.Models
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal)
            : base(principal)
        {

        }

        public int Id
        {
            get { return Convert.ToInt32(FindFirst(ClaimTypes.NameIdentifier).Value); }
        }
        public string Nome
        {
            get { return FindFirst(ClaimTypes.Name).Value; }
        }
        public string Email
        {
            get { return FindFirst(ClaimTypes.Email).Value; }
        }
        public int PerfilId
        {
            get { return Convert.ToInt32(FindFirst("PerfilId").Value); }
        }
        public int TimeId
        {
            get { return Convert.ToInt32(FindFirst("TimeId").Value); }
        }
    }
}