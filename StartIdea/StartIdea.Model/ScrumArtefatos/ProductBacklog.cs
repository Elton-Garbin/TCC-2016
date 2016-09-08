using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public enum StoryPoint
    {
        N = 0,
        PP = 1,
        P = 3,
        M = 5,
        G = 8,
        GG = 13
    }

    public class ProductBacklog
    {
        public ProductBacklog()
        {
            InteracoesProductBacklogItem = new HashSet<InteracaoProductBacklogItem>();
            SprintsBacklog = new HashSet<SprintBacklog>();

            DataInclusao = DateTime.Now;
            StoryPoint = StoryPoint.N;
        }

        public int Id { get; set; }
        public string UserStory { get; set; }
        public StoryPoint StoryPoint { get; set; }
        public short Prioridade { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataExclusao { get; set; }
        public int ProductOwnerId { get; set; }

        public virtual ProductOwner ProductOwner { get; set; }

        public virtual ICollection<InteracaoProductBacklogItem> InteracoesProductBacklogItem { get; set; }
        public virtual ICollection<SprintBacklog> SprintsBacklog { get; set; }
    }
}
