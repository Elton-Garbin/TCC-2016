using StartIdea.Model.TimeScrum;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.ModelMapping.TimeScrum
{
    public class TimeMap : EntityTypeConfiguration<Time>
    {
        public TimeMap()
        {
            Property(x => x.Nome)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}