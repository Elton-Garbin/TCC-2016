using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class ProductBacklog
    {
        public ProductBacklog()
        {
            ProductBacklogItem = new HashSet<ProductBacklogItem>();
        }

        public int Id { get; set; }
        //[StringLength(150)]
        public string Descricao { get; set; }
        public int ProductOwnerId { get; set; }

        public virtual ProductOwner ProductOwner { get; set; }

        public virtual ICollection<ProductBacklogItem> ProductBacklogItem { get; set; }
    }
}