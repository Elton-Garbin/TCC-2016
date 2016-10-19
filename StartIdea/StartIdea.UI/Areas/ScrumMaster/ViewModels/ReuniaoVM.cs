using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.ScrumMaster.ViewModels
{
    public class ReuniaoVM
    {
        public ReuniaoVM()
        {
            DataInicial = DateTime.Now;
            DataFinal = DateTime.Now;
        }

        #region Properties
        public string ActionForm { get; private set; }
        public string SubmitValue { get; private set; }

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;

                SubmitValue = (_Id > 0) ? "Editar" : "Cadastrar";
                ActionForm = (_Id > 0) ? "Edit" : "Create";
            }
        }

        [Required(ErrorMessage = "Campo Local obrigatório.")]
        [StringLength(50, ErrorMessage = "Campo Local deve ter no máximo 50 caracteres.")]
        public string Local { get; set; }

        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "Campo Data Inicial obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "Campo Data Inicial inválido.")]
        public DateTime DataInicial { get; set; }

        [DisplayName("Data Final")]
        [Required(ErrorMessage = "Campo Data Final obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "Campo Data Final inválido.")]
        public DateTime DataFinal { get; set; }

        [Required(ErrorMessage = "Campo Ata obrigatório.")]
        [DataType(DataType.MultilineText)]
        public string Ata { get; set; }

        public int SprintId { get; set; }
        #endregion
    }
}