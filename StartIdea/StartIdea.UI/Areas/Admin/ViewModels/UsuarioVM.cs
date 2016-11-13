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
            IsAdmin = false;
            NaoEnviouEmail = false;
        }

        #region Properties
        #region Filter
        public IPagedList<Usuario> UsuarioList { get; set; }
        public int? PaginaGrid { get; set; }
        #endregion
        public bool NaoEnviouEmail { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Campo E-mail obrigatório.")]
        [StringLength(256, ErrorMessage = "Campo E-mail deve ter no máximo 256 caracteres.")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", ErrorMessage = "Campo E-mail inválido.")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Usuário obrigatório.")]
        [StringLength(20, ErrorMessage = "Campo Usuário deve ter no máximo 20 caracteres.")]
        [RegularExpression(@"^(?=.{2,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "Campo Usuário inválido.")]
        [DisplayName("Usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Campo CPF obrigatório.")]
        [CustomCPF(ErrorMessage = "Campo CPF inválido.")]
        public string CPF { get; set; }

        [DisplayName("Ativo")]
        public bool IsActive { get; set; }

        [DisplayName("Admin")]
        public bool IsAdmin { get; set; }
        #endregion
    }
}