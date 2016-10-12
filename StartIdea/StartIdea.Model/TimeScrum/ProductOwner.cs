using StartIdea.Model.ScrumArtefatos;
using System.Collections.Generic;

namespace StartIdea.Model.TimeScrum
{
    public class ProductOwner
    {
        public ProductOwner()
        {
            ProductBacklogs = new HashSet<ProductBacklog>();
        }

        #region Fields
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        #endregion

        #region References
        public virtual Usuario Usuario { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<ProductBacklog> ProductBacklogs { get; set; }
        #endregion
    }
}
