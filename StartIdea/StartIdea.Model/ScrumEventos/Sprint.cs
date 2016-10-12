using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.Model.ScrumEventos
{
    public class Sprint
    {
        public Sprint()
        {
            Reunioes = new HashSet<Reuniao>();
            SprintBacklogs = new HashSet<SprintBacklog>();
            DataInicial = DateTime.Now;
            DataFinal = DateTime.Now.AddDays(30);
            DataCadastro = DateTime.Now;
        }

        #region Fields
        public int Id { get; set; }

        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "Campo Data Inicial obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "Campo Data Inicial inválido."), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime DataInicial { get; set; }

        [DisplayName("Data Final")]
        [Required(ErrorMessage = "Campo Data Final obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "Campo Data Final inválido."), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataFinal { get; set; }

        [Required(ErrorMessage = "Campo Objetivo obrigatório.")]
        [StringLength(75, ErrorMessage = "Campo User Story deve ter no máximo 75 caracteres.")]
        public string Objetivo { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime? DataCancelamento { get; set; }

        [DataType(DataType.MultilineText)]
        public string MotivoCancelamento { get; set; }

        public int TimeId { get; set; }
        #endregion

        #region References
        public virtual Time Time { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Reuniao> Reunioes { get; set; }
        public virtual ICollection<SprintBacklog> SprintBacklogs { get; set; }
        #endregion
    }
}
