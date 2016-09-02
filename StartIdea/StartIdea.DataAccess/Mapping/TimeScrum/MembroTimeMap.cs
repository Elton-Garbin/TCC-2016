using StartIdea.Model.TimeScrum;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.Mapping.TimeScrum
{
    public class MembroTimeMap : EntityTypeConfiguration<MembroTime>
    {
        public MembroTimeMap()
        {
            Property(x => x.Funcao)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}