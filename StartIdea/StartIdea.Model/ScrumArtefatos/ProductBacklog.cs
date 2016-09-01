using StartIdea.Model.TimeScrum;
using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class ProductBacklog
    {
        public ProductBacklog()
        {
            ProductBacklogItems = new HashSet<ProductBacklogItem>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public int ProductOwnerId { get; set; }

        public virtual ProductOwner ProductOwner { get; set; }

        public virtual ICollection<ProductBacklogItem> ProductBacklogItems { get; set; }
    }
}