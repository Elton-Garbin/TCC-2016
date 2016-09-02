using StartIdea.Model.ScrumEventos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.Mapping.ScrumEventos
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