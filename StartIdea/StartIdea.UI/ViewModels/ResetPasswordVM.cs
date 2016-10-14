using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.ViewModels
{
    public class ResetPasswordVM
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Senha obrigatório.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{4,}$", ErrorMessage = "Senha deve ser composta de letra maíscula, minúscula, número e caracteres especiais.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "Campo Confirmação da Senha diferente do campo Senha.")]
        [DataType(DataType.Password)]
        public string ConfirmaSenha { get; set; }

        public string TokenActivation { get; set; }
    }
}