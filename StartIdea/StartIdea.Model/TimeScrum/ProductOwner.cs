using StartIdea.Model.ScrumArtefatos;
using System.Collections.Generic;

namespace StartIdea.Model.TimeScrum
{
    public class ProductOwner
    {
        public ProductOwner()
        {
            ProductsBacklog = new HashSet<ProductBacklog>();
        }

        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<ProductBacklog> ProductsBacklog { get; set; }
    }
}
