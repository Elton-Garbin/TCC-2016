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
            DailyScrum = new HashSet<DailyScrum>();
            PlanejamentoSprint = new HashSet<PlanejamentoSprint>();
            RetrospectivaSprint = new HashSet<RetrospectivaSprint>();
            RevisaoSprint = new HashSet<RevisaoSprint>();
            SprintBacklog = new HashSet<SprintBacklog>();
        }

        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        //[Required]
        //[StringLength(50)]
        public string Objetivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public int TimeId { get; set; }

        public virtual Time Time { get; set; }

        public virtual ICollection<DailyScrum> DailyScrum { get; set; }
        public virtual ICollection<PlanejamentoSprint> PlanejamentoSprint { get; set; }
        public virtual ICollection<RetrospectivaSprint> RetrospectivaSprint { get; set; }
        public virtual ICollection<RevisaoSprint> RevisaoSprint { get; set; }
        public virtual ICollection<SprintBacklog> SprintBacklog { get; set; }
    }
}