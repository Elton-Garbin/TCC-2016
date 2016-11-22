using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.Admin.ViewModels
{
    public enum TimeScrum
    {
        TimeDesenvolvimento = 1,
        ProductOwner = 2,
        ScrumMaster = 3,
    }

    public class PerfilVM
    {
        public PerfilVM()
        {
            Papel = TimeScrum.TimeDesenvolvimento;
            Id = 0;
        }

        public string ProductOwner { get; set; }
        public string ScrumMaster { get; set; }
        public string SubmitValue { get; private set; }
        public int UsuarioId { get; set; }

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;

                SubmitValue = (_Id > 0) ? "Editar" : "Cadastrar";
            }
        }

        public TimeScrum Papel { get; set; }

        [DisplayName("Função")]
        [StringLength(20, ErrorMessage = "Campo Função deve ter no máximo 20 caracteres.")]
        public string Descricao { get; set; }
    }
}