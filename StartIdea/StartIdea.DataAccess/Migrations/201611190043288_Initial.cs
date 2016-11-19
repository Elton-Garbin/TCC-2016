namespace StartIdea.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 20),
                        Classificacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatusTarefa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataInclusao = c.DateTime(nullable: false),
                        TarefaId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        MembroTimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tarefa", t => t.TarefaId)
                .ForeignKey("dbo.MembroTime", t => t.MembroTimeId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.TarefaId)
                .Index(t => t.StatusId)
                .Index(t => t.MembroTimeId);
            
            CreateTable(
                "dbo.MembroTime",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Funcao = c.String(nullable: false, maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        DataManutencao = c.DateTime(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                        TimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .ForeignKey("dbo.Time", t => t.TimeId)
                .Index(t => t.UsuarioId)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.HistoricoEstimativa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoryPoint = c.Int(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        ProductBacklogId = c.Int(nullable: false),
                        MembroTimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MembroTime", t => t.MembroTimeId)
                .ForeignKey("dbo.ProductBacklog", t => t.ProductBacklogId)
                .Index(t => t.ProductBacklogId)
                .Index(t => t.MembroTimeId);
            
            CreateTable(
                "dbo.ProductBacklog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserStory = c.String(nullable: false, maxLength: 150),
                        StoryPoint = c.Int(nullable: false),
                        Prioridade = c.Short(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        ProductOwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductOwner", t => t.ProductOwnerId)
                .Index(t => t.ProductOwnerId);
            
            CreateTable(
                "dbo.ProductOwner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        DataManutencao = c.DateTime(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 256),
                        Senha = c.String(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 20),
                        CPF = c.String(nullable: false, maxLength: 11),
                        Foto = c.Binary(),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        TokenActivation = c.Guid(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "UK_Usuario_Email")
                .Index(t => t.UserName, unique: true, name: "UK_Usuario_UserName");
            
            CreateTable(
                "dbo.ScrumMaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        DataManutencao = c.DateTime(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                        TimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Time", t => t.TimeId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Time",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sprint",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataInicial = c.DateTime(nullable: false),
                        DataFinal = c.DateTime(nullable: false),
                        Objetivo = c.String(nullable: false, maxLength: 75),
                        DataCadastro = c.DateTime(nullable: false),
                        DataCancelamento = c.DateTime(),
                        MotivoCancelamento = c.String(),
                        TimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Time", t => t.TimeId)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Reuniao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TipoReuniao = c.Int(nullable: false),
                        Local = c.String(nullable: false, maxLength: 50),
                        DataInicial = c.DateTime(nullable: false),
                        DataFinal = c.DateTime(nullable: false),
                        Ata = c.String(nullable: false),
                        SprintId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sprint", t => t.SprintId)
                .Index(t => t.SprintId);
            
            CreateTable(
                "dbo.SprintBacklog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataCancelamento = c.DateTime(),
                        MotivoCancelamento = c.String(),
                        SprintId = c.Int(nullable: false),
                        ProductBacklogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductBacklog", t => t.ProductBacklogId)
                .ForeignKey("dbo.Sprint", t => t.SprintId)
                .Index(t => t.SprintId)
                .Index(t => t.ProductBacklogId);
            
            CreateTable(
                "dbo.Tarefa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        SprintBacklogId = c.Int(nullable: false),
                        MembroTimeId = c.Int(nullable: false),
                        DataCancelamento = c.DateTime(),
                        MotivoCancelamento = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MembroTime", t => t.MembroTimeId)
                .ForeignKey("dbo.SprintBacklog", t => t.SprintBacklogId)
                .Index(t => t.SprintBacklogId)
                .Index(t => t.MembroTimeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StatusTarefa", "StatusId", "dbo.Status");
            DropForeignKey("dbo.StatusTarefa", "MembroTimeId", "dbo.MembroTime");
            DropForeignKey("dbo.ScrumMaster", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Sprint", "TimeId", "dbo.Time");
            DropForeignKey("dbo.StatusTarefa", "TarefaId", "dbo.Tarefa");
            DropForeignKey("dbo.Tarefa", "SprintBacklogId", "dbo.SprintBacklog");
            DropForeignKey("dbo.Tarefa", "MembroTimeId", "dbo.MembroTime");
            DropForeignKey("dbo.SprintBacklog", "SprintId", "dbo.Sprint");
            DropForeignKey("dbo.SprintBacklog", "ProductBacklogId", "dbo.ProductBacklog");
            DropForeignKey("dbo.Reuniao", "SprintId", "dbo.Sprint");
            DropForeignKey("dbo.ScrumMaster", "TimeId", "dbo.Time");
            DropForeignKey("dbo.MembroTime", "TimeId", "dbo.Time");
            DropForeignKey("dbo.ProductOwner", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.MembroTime", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.ProductBacklog", "ProductOwnerId", "dbo.ProductOwner");
            DropForeignKey("dbo.HistoricoEstimativa", "ProductBacklogId", "dbo.ProductBacklog");
            DropForeignKey("dbo.HistoricoEstimativa", "MembroTimeId", "dbo.MembroTime");
            DropIndex("dbo.Tarefa", new[] { "MembroTimeId" });
            DropIndex("dbo.Tarefa", new[] { "SprintBacklogId" });
            DropIndex("dbo.SprintBacklog", new[] { "ProductBacklogId" });
            DropIndex("dbo.SprintBacklog", new[] { "SprintId" });
            DropIndex("dbo.Reuniao", new[] { "SprintId" });
            DropIndex("dbo.Sprint", new[] { "TimeId" });
            DropIndex("dbo.ScrumMaster", new[] { "TimeId" });
            DropIndex("dbo.ScrumMaster", new[] { "UsuarioId" });
            DropIndex("dbo.Usuario", "UK_Usuario_UserName");
            DropIndex("dbo.Usuario", "UK_Usuario_Email");
            DropIndex("dbo.ProductOwner", new[] { "UsuarioId" });
            DropIndex("dbo.ProductBacklog", new[] { "ProductOwnerId" });
            DropIndex("dbo.HistoricoEstimativa", new[] { "MembroTimeId" });
            DropIndex("dbo.HistoricoEstimativa", new[] { "ProductBacklogId" });
            DropIndex("dbo.MembroTime", new[] { "TimeId" });
            DropIndex("dbo.MembroTime", new[] { "UsuarioId" });
            DropIndex("dbo.StatusTarefa", new[] { "MembroTimeId" });
            DropIndex("dbo.StatusTarefa", new[] { "StatusId" });
            DropIndex("dbo.StatusTarefa", new[] { "TarefaId" });
            DropTable("dbo.Tarefa");
            DropTable("dbo.SprintBacklog");
            DropTable("dbo.Reuniao");
            DropTable("dbo.Sprint");
            DropTable("dbo.Time");
            DropTable("dbo.ScrumMaster");
            DropTable("dbo.Usuario");
            DropTable("dbo.ProductOwner");
            DropTable("dbo.ProductBacklog");
            DropTable("dbo.HistoricoEstimativa");
            DropTable("dbo.MembroTime");
            DropTable("dbo.StatusTarefa");
            DropTable("dbo.Status");
        }
    }
}
