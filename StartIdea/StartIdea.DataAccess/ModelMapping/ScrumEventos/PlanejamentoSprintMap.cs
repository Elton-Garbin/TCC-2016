using StartIdea.Model.ScrumEventos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.ModelMapping.ScrumEventos
{
    public class PlanejamentoSprintMap : EntityTypeConfiguration<PlanejamentoSprint>
    {
        public PlanejamentoSprintMap()
        {
            Property(x => x.Local)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}