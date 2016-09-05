using StartIdea.Model.ScrumArtefatos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.Mapping.ScrumArtefatos
{
    public class InteracaoProductBacklogItemMap : EntityTypeConfiguration<InteracaoProductBacklogItem>
    {
        public InteracaoProductBacklogItemMap()
        {
            Property(x => x.Descricao)
                .IsRequired();
        }
    }
}
