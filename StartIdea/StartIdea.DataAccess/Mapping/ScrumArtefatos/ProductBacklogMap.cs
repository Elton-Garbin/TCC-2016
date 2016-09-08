using StartIdea.Model.ScrumArtefatos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.Mapping.ScrumArtefatos
{
    public class ProductBacklogMap : EntityTypeConfiguration<ProductBacklog>
    {
        public ProductBacklogMap()
        {
            Property(x => x.UserStory)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
