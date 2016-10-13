using StartIdea.DataAccess.Migrations;
using StartIdea.Model;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.Model.TimeScrum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace StartIdea.DataAccess
{
    public class StartIdeaDBContext : DbContext
    {
        public StartIdeaDBContext()
            : base("StartIdeaDB")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StartIdeaDBContext, Configuration>("StartIdeaDB"));
        }

        public DbSet<Usuario> Usuarios { get; set; }

        #region TimeScrum
        public DbSet<ProductOwner> ProductOwners { get; set; }
        public DbSet<ScrumMaster> ScrumMasters { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<MembroTime> MembrosTime { get; set; }
        #endregion

        #region ScrumEventos
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Reuniao> Reunioes { get; set; }
        #endregion

        #region ScrumArtefatos
        public DbSet<ProductBacklog> ProductBacklogs { get; set; }
        public DbSet<HistoricoEstimativa> HistoricoEstimativas { get; set; }
        public DbSet<SprintBacklog> SprintBacklogs { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Status> AllStatus { get; set; }
        public DbSet<StatusTarefa> StatusTarefas { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Usuario>()
                .Property(p => p.Email)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("UK_Usuario_Email") { IsUnique = true }));

            modelBuilder.Entity<Usuario>()
                .Property(p => p.UserName)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("UK_Usuario_UserName") { IsUnique = true }));
        }
    }
}
