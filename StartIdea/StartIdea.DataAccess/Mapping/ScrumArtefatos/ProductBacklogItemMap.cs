using StartIdea.Model.ScrumArtefatos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.Mapping.ScrumArtefatos
{
    public class ProductBacklogItemMap : EntityTypeConfiguration<ProductBacklogItem>
    {
        public ProductBacklogItemMap()
        {
            Property(x => x.UserStory)
                .HasMaxLength(50)
                .IsRequired();

            Property(x => x.Tamanho)
                .HasMaxLength(1)
                .IsFixedLength();
        }
    }
}
