using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class ProductBacklogItem
    {
        public ProductBacklogItem()
        {
            SprintBacklog = new HashSet<SprintBacklog>();
        }

        public int Id { get; set; }
        //[Required]
        //[StringLength(50)]
        public string UserStory { get; set; }
        //[StringLength(1)]
        public string Tamanho { get; set; }
        public int? Prioridade { get; set; }
        public int ProductBacklogId { get; set; }

        public virtual ProductBacklog ProductBacklog { get; set; }

        public virtual ICollection<SprintBacklog> SprintBacklog { get; set; }
    }
}
