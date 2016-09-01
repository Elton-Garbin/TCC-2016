using StartIdea.Model.ScrumArtefatos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.ModelMapping.ScrumArtefatos
{
    public class TarefaMap : EntityTypeConfiguration<Tarefa>
    {
        public TarefaMap()
        {
            Property(x => x.Descricao)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}