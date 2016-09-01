using StartIdea.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.ModelMapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            Property(x => x.UserName)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.Senha)
                .HasMaxLength(50)
                .IsRequired();

            Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}