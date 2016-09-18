using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;

namespace StartIdea.Model.ScrumEventos
{
    public class Sprint
    {
        public Sprint()
        {
            Reunioes = new HashSet<Reuniao>();
            SprintBacklogs = new HashSet<SprintBacklog>();

            #region Default Values
            DataInicio   = DateTime.Now;
            DataFim      = DateTime.Now.AddDays(30);
            DataCadastro = DateTime.Now;
            #endregion
        }

        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Objetivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public int TimeId { get; set; }

        public virtual Time Time { get; set; }

        public virtual ICollection<Reuniao> Reunioes { get; set; }
        public virtual ICollection<SprintBacklog> SprintBacklogs { get; set; }
    }
}
