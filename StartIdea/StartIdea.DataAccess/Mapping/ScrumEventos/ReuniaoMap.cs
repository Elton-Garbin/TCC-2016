using StartIdea.Model.ScrumEventos;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.Mapping.ScrumEventos
{
    public class ReuniaoMap : EntityTypeConfiguration<Reuniao>
    {
        public ReuniaoMap()
        {
            Property(x => x.Local)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
