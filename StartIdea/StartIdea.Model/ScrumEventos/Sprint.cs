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
            DailiesScrum = new HashSet<DailyScrum>();
            PlanejamentosSprint = new HashSet<PlanejamentoSprint>();
            RetrospectivasSprint = new HashSet<RetrospectivaSprint>();
            RevisoesSprint = new HashSet<RevisaoSprint>();
            SprintBacklogs = new HashSet<SprintBacklog>();
        }

        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Objetivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public int TimeId { get; set; }

        public virtual Time Time { get; set; }

        public virtual ICollection<DailyScrum> DailiesScrum { get; set; }
        public virtual ICollection<PlanejamentoSprint> PlanejamentosSprint { get; set; }
        public virtual ICollection<RetrospectivaSprint> RetrospectivasSprint { get; set; }
        public virtual ICollection<RevisaoSprint> RevisoesSprint { get; set; }
        public virtual ICollection<SprintBacklog> SprintBacklogs { get; set; }
    }
}