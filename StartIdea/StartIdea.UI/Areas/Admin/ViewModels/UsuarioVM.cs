using PagedList;
using StartIdea.Model;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.Admin.ViewModels
{
    public class UsuarioVM
    {
        #region Properties
        #region Filter
        public IPagedList<Usuario> UsuarioList { get; set; }
        public int? PaginaGrid { get; set; }
        public string DisplayCreate { get; set; }
        public string DisplayEdit { get; set; }
        #endregion

        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Email { get; set; }

        [Required, MaxLength(256)]
        public string UserName { get; set; }

        [Required, StringLength(11, MinimumLength = 11)]
        public string CPF { get; set; }

        public bool IsActive { get; set; }
        #endregion
    }
}