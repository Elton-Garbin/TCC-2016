using StartIdea.Model.ScrumArtefatos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.Mapping.ScrumArtefatos
{
    public class TarefaMap : EntityTypeConfiguration<Tarefa>
    {
        public TarefaMap()
        {
            Property(x => x.Descricao)
                .IsRequired();
        }
    }
}
