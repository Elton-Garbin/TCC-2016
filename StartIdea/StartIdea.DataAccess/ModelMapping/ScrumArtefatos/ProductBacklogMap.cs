using StartIdea.Model.ScrumArtefatos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.ModelMapping.ScrumArtefatos
{
    public class ProductBacklogMap : EntityTypeConfiguration<ProductBacklog>
    {
        public ProductBacklogMap()
        {
            Property(x => x.Descricao)
                .HasMaxLength(150);
        }
    }
}