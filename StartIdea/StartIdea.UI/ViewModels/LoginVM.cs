using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.ViewModels
{
    public class LoginVM
    {
        public LoginVM()
        {
            PermanecerConectado = false;
        }

        [Required(ErrorMessage = "Campo E-mail obrigatório.")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", ErrorMessage = "Campo E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Senha obrigatório.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DisplayName("Mantenha-me conectado")]
        public bool PermanecerConectado { get; set; }
    }
}