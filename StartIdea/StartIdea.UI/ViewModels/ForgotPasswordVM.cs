using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.ViewModels
{
    public class ForgotPasswordVM
    {
        public ForgotPasswordVM()
        {
            CssClassMessage = "text-danger";
        }

        [Required(ErrorMessage = "Campo E-mail obrigatório.")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", ErrorMessage = "Campo E-mail inválido.")]
        public string Email { get; set; }

        public string CssClassMessage { get; set; }
    }
}