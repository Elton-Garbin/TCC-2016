using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.ProductOwner.ViewModels
{
    public class SprintVM
    {
        public int Id { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Objetivo { get; set; }
        public DateTime DataCadastro { get; set; }

        [DisplayName("Motivo Cancelamento")]
        [Required(ErrorMessage = "Campo Motivo Cancelamento obrigatório.")]
        [DataType(DataType.MultilineText)]
        public string MotivoCancelamento { get; set; }
    }
}