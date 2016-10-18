using PagedList;
using StartIdea.Model;
using StartIdea.UI.Areas.Admin.Models;
using System.ComponentModel;
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

        [Required(ErrorMessage = "Informe o Email"), MaxLength(256)]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", ErrorMessage = "Campo E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe o Apelido do usuário"), MaxLength(256)]
        [DisplayName("Apelido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Informe o CPF"), StringLength(11, MinimumLength = 11)]
        [CustomCPF(ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        [DisplayName("Ativo")]
        public bool IsActive { get; set; }
        #endregion
    }
}