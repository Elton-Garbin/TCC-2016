using StartIdea.DataAccess.Mapping;
using StartIdea.DataAccess.Mapping.ScrumArtefatos;
using StartIdea.DataAccess.Mapping.ScrumEventos;
using StartIdea.DataAccess.Mapping.TimeScrum;
using StartIdea.Model;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.Model.TimeScrum;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace StartIdea.DataAccess
{
    public partial class StartIdeaContext : DbContext
    {
        public StartIdeaContext()
            : base("name=StartIdeaContext")
        {
            Configuration.LazyLoadingEnabled   = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<DailyScrum> DailiesScrum { get; set; }
        public virtual DbSet<MembroTime> MembrosTime { get; set; }
        public virtual DbSet<PlanejamentoSprint> PlanejamentosSprint { get; set; }
        public virtual DbSet<ProductOwner> ProductOwners { get; set; }
        public virtual DbSet<ProductBacklog> ProductBacklogs { get; set; }
        public virtual DbSet<ProductBacklogItem> ProductBacklogItems { get; set; }
        public virtual DbSet<RetrospectivaSprint> RetrospectivasSprint { get; set; }
        public virtual DbSet<RevisaoSprint> RevisoesSprint { get; set; }
        public virtual DbSet<ScrumMaster> ScrumMasters { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<SprintBacklog> SprintBacklogs { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Tarefa> Tarefas { get; set; }
        public virtual DbSet<TarefaStatus> TarefasStatus { get; set; }
        public virtual DbSet<Time> Times { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UsuarioMap());

            // ScrumArtefatos
            modelBuilder.Configurations.Add(new ProductBacklogItemMap());
            modelBuilder.Configurations.Add(new ProductBacklogMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new TarefaMap());

            // ScrumEventos
            modelBuilder.Configurations.Add(new DailyScrumMap());
            modelBuilder.Configurations.Add(new PlanejamentoSprintMap());
            modelBuilder.Configurations.Add(new RetrospectivaSprintMap());
            modelBuilder.Configurations.Add(new RevisaoSprintMap());
            modelBuilder.Configurations.Add(new SprintMap());

            // TimeScrum
            modelBuilder.Configurations.Add(new MembroTimeMap());
            modelBuilder.Configurations.Add(new TimeMap());
        }
    }
}
