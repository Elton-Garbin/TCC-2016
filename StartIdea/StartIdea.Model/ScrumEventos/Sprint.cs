using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;
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
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }

        [Required]
        [MaxLength(75)]
        public string Objetivo { get; set; }

        public DateTime DataCadastro { get; set; }
        public DateTime? DataCancelamento { get; set; }
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
