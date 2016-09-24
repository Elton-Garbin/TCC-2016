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

            #region Default Values
            DataInicial = DateTime.Now;
            DataFinal = DateTime.Now.AddDays(30);
            DataCadastro = DateTime.Now;
            #endregion
        }

        #region Campos
        public int Id { get; set; }

        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "Campo Data Inicial obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "Campo Data Inicial inválido.")]
        public DateTime DataInicial { get; set; }

        [DisplayName("Data Final")]
        [Required(ErrorMessage = "Campo Data Final obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "Campo Data Final inválido.")]
        public DateTime DataFinal { get; set; }

        [Required(ErrorMessage = "Campo Objetivo obrigatório.")]
        [StringLength(75, ErrorMessage = "Campo User Story deve ter no máximo 75 caracteres.")]
        public string Objetivo { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime? DataCancelamento { get; set; }

        public string MotivoCancelamento { get; set; }

        public int TimeId { get; set; }
        #endregion

        public virtual Time Time { get; set; }

        public virtual ICollection<Reuniao> Reunioes { get; set; }
        public virtual ICollection<SprintBacklog> SprintBacklogs { get; set; }
    }
}
