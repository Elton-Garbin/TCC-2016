using StartIdea.Model.ScrumEventos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.ModelMapping.ScrumEventos
{
    public class SprintMap : EntityTypeConfiguration<Sprint>
    {
        public SprintMap()
        {
            Property(x => x.Objetivo)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}