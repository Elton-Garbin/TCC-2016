using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.ViewModels
{
    public class UsuarioVM
    {
        public UsuarioVM()
        {
            CssClassMessage = "text-danger";
        }

        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Campo Nova Senha obrigatório.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{4,}$", ErrorMessage = "Nova senha deve ser composta de letra maíscula, minúscula, número e caracteres especiais.")]
        [DisplayName("Nova Senha")]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [Compare("NovaSenha", ErrorMessage = "Campo Confirmação da Nova Senha diferente do campo Nova Senha.")]
        [DisplayName("Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        public string ConfirmaNovaSenha { get; set; }

        public string CssClassMessage { get; set; }
    }
}