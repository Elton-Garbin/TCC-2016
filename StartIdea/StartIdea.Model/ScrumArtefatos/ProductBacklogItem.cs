using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class ProductBacklogItem
    {
        public ProductBacklogItem()
        {
            SprintBacklogs = new HashSet<SprintBacklog>();
        }

        public int Id { get; set; }
        public string UserStory { get; set; }
        public string Tamanho { get; set; }
        public int? Prioridade { get; set; }
        public int ProductBacklogId { get; set; }

        public virtual ProductBacklog ProductBacklog { get; set; }

        public virtual ICollection<SprintBacklog> SprintBacklogs { get; set; }
    }
}
