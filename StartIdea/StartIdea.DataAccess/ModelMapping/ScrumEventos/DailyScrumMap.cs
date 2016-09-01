using StartIdea.Model.ScrumEventos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.ModelMapping.ScrumEventos
{
    public class DailyScrumMap : EntityTypeConfiguration<DailyScrum>
    {
        public DailyScrumMap()
        {
            Property(x => x.Local)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}