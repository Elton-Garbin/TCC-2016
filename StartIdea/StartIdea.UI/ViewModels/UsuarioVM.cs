using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace StartIdea.UI.ViewModels
{
    public class UsuarioVM : IValidatableObject
    {
        public UsuarioVM()
        {
            CssClassMessage = "text-danger";
            TrocarSenha = false;
        }

        public string CssClassMessage { get; set; }

        public int Id { get; set; }
        public string ImageBase64 { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }

        [DisplayName("Trocar Senha")]
        public bool TrocarSenha { get; set; }

        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{4,}$", ErrorMessage = "Nova senha deve ser composta de letra maíscula, minúscula, número e caracteres especiais.")]
        [DisplayName("Nova Senha")]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [Compare("NovaSenha", ErrorMessage = "Campo Confirmar Nova Senha diferente do campo Nova Senha.")]
        [DisplayName("Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        public string ConfirmaNovaSenha { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TrocarSenha)
            {
                if (string.IsNullOrEmpty(Senha))
                {
                    yield return
                        new ValidationResult(errorMessage: "Campo Senha obrigatório.",
                                             memberNames: new[] { "Senha" });
                }

                if (string.IsNullOrEmpty(NovaSenha))
                {
                    yield return
                        new ValidationResult(errorMessage: "Campo Nova Senha obrigatório.",
                                             memberNames: new[] { "NovaSenha" });
                }

                if (string.IsNullOrEmpty(ConfirmaNovaSenha))
                {
                    yield return
                        new ValidationResult(errorMessage: "Campo Confirmar Nova Senha obrigatório.",
                                             memberNames: new[] { "ConfirmaNovaSenha" });
                }
            }
        }
    }
}