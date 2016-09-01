using StartIdea.Model.ScrumEventos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.ModelMapping.ScrumEventos
{
    public class RevisaoSprintMap : EntityTypeConfiguration<RevisaoSprint>
    {
        public RevisaoSprintMap()
        {
            Property(x => x.Local)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}