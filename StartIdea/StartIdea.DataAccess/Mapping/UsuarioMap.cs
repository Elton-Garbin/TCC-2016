using StartIdea.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace StartIdea.DataAccess.Mapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            Property(x => x.UserName)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("UK_Usuario_UserName") { IsUnique = true }));

            Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.Senha)
                .IsRequired();

            Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
