using PagedList;
using StartIdea.Model;
using StartIdea.UI.Areas.Admin.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.Admin.ViewModels
{
    public class UsuarioVM
    {
        public UsuarioVM()
        {
            IsActive = true;
        }

        #region Properties
        #region Filter
        public IPagedList<Usuario> UsuarioList { get; set; }
        public int? PaginaGrid { get; set; }
        #endregion

        public int Id { get; set; }

        [Required(ErrorMessage = "Campo E-mail obrigatório."), MaxLength(256)]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", ErrorMessage = "Campo E-mail inválido.")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Usuário obrigatório."), MaxLength(256)]
        [DisplayName("Usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Campo CPF obrigatório."), StringLength(11, MinimumLength = 11)]
        [CustomCPF(ErrorMessage = "Campo CPF inválido.")]
        public string CPF { get; set; }

        [DisplayName("Ativo")]
        public bool IsActive { get; set; }
        #endregion
    }
}