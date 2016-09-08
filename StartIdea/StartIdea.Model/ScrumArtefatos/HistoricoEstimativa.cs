using StartIdea.Model.TimeScrum;
using System;

namespace StartIdea.Model.ScrumArtefatos
{
    public class HistoricoEstimativa
    {
        public HistoricoEstimativa()
        {
            DataInclusao = DateTime.Now;
        }

        public int Id { get; set; }
        public StoryPoint StoryPoint { get; set; }
        public DateTime DataInclusao { get; set; }
        public int ProductBacklogId { get; set; }
        public int MembroTimeId { get; set; }

        public virtual ProductBacklog ProductBacklog { get; set; }
        public virtual MembroTime MembroTime { get; set; }
    }
}