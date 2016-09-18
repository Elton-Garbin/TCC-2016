using StartIdea.Model;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace StartIdea.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<StartIdeaDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(StartIdeaDBContext context)
        {
            #region Usuarios
            context.Usuarios.AddOrUpdate(x => x.Id,
                new Usuario()
                {
                    Id = 1,
                    Email = "eltongarbin@gmail.com",
                    Senha = "1234",
                    UserName = "EltonGarbin",
                    Nome = "Elton Diego Garbin do Nascimento"
                },
                new Usuario()
                {
                    Id = 2,
                    Email = "gabriel.silva@newconsoftware.com.br",
                    Senha = "1234",
                    UserName = "GabrielXFD",
                    Nome = "Gabriel Fernandez da Silva"
                },
                new Usuario()
                {
                    Id = 3,
                    Email = "arieldalton@gmail.com",
                    Senha = "1234",
                    UserName = "ArielDalton",
                    Nome = "Ariel Dalton Garbin do Nascimento"
                },
                new Usuario()
                {
                    Id = 4,
                    Email = "rafael.pessotti@newconsoftware.com.br",
                    Senha = "1234",
                    UserName = "Pessotti",
                    Nome = "Rafael Pessotti Magdaleno"
                },
                new Usuario()
                {
                    Id = 5,
                    Email = "jose.silva@gmail.com",
                    Senha = "1234",
                    UserName = "J.Silva",
                    Nome = "José da Silva Santos"
                }
            );
            #endregion

            #region ProductOwner
            context.ProductOwners.AddOrUpdate(x => x.Id,
                new ProductOwner() { Id = 1, UsuarioId = 2 }
            );

            context.ProductBacklogs.AddOrUpdate(x => x.Id,
                new ProductBacklog()
                {
                    Id = 1,
                    UserStory = "Eu como um consorciado desejo realizar o login no web atendimento para entrar na minha área restrita.",
                    DataInclusao = new DateTime(2016, 6, 1, 9, 00, 00),
                    Prioridade = 1,
                    StoryPoint = StoryPoint.P,
                    ProductOwnerId = 1
                },
                new ProductBacklog()
                {
                    Id = 3,
                    UserStory = "Eu como um consorciado desejo sair da minha área restrita, ou seja realizar o logout.",
                    DataInclusao = new DateTime(2016, 6, 1, 9, 30, 00),
                    Prioridade = 2,
                    StoryPoint = StoryPoint.PP,
                    ProductOwnerId = 1
                }
            );

            context.HistoricoEstimativas.AddOrUpdate(x => x.Id,
                new HistoricoEstimativa()
                {
                    Id = 1,
                    DataInclusao = new DateTime(2016, 6, 1, 9, 10, 00),
                    MembroTimeId = 1,
                    ProductBacklogId = 1,
                    StoryPoint = StoryPoint.P
                },
                new HistoricoEstimativa()
                {
                    Id = 1,
                    DataInclusao = new DateTime(2016, 6, 1, 9, 35, 00),
                    MembroTimeId = 2,
                    ProductBacklogId = 3,
                    StoryPoint = StoryPoint.PP
                }
            );
            #endregion

            #region ScrumMaster
            context.ScrumMasters.AddOrUpdate(x => x.Id,
                new ScrumMaster() { Id = 1, UsuarioId = 1 }
            );

            context.AllStatus.AddOrUpdate(x => x.Id,
                new Status() { Id = 1, Classificacao = Classificacao.Ready, Descricao = "À fazer" },
                new Status() { Id = 2, Classificacao = Classificacao.InProgress, Descricao = "Fazendo" },
                new Status() { Id = 3, Classificacao = Classificacao.Done, Descricao = "Feito" }
            );

            context.Times.AddOrUpdate(x => x.Id,
                new Time() { Id = 1, Nome = "Time de Desenvolvimento", ScrumMasterId = 1 }
            );

            context.MembrosTime.AddOrUpdate(x => x.Id,
                new MembroTime() { Id = 1, Funcao = "Analista", TimeId = 1, UsuarioId = 3 },
                new MembroTime() { Id = 2, Funcao = "Desenvolvedor", TimeId = 1, UsuarioId = 4 },
                new MembroTime() { Id = 3, Funcao = "Estagiário", TimeId = 1, UsuarioId = 5 }
            );

            #region Sprint Generator
            context.Sprints.AddOrUpdate(x => x.Id,
                new Sprint()
                {
                    Id = 1,
                    Objetivo = "Criar processo de login",
                    DataCadastro = new DateTime(2016, 6, 2, 9, 30, 00),
                    DataInicio = new DateTime(2016, 6, 6, 8, 00, 00),
                    DataFim = new DateTime(2016, 7, 1, 18, 00, 00),
                    TimeId = 1
                }
                //new Sprint()
                //{
                //    Id = 2,
                //    Objetivo = "Criar processo de devolução de valores pagos",
                //    DataCadastro = new DateTime(2016, 7, 4, 13, 45, 00),
                //    DataInicio = new DateTime(2016, 8, 1, 8, 00, 00),
                //    DataFim = new DateTime(2016, 9, 2, 18, 00, 00),
                //    DataCancelamento = new DateTime(2016, 9, 1, 8, 30, 00),
                //    TimeId = 1
                //},
                //new Sprint()
                //{
                //    Id = 3,
                //    Objetivo = "Criar processo para oferta de lance",
                //    DataCadastro = new DateTime(2016, 9, 1, 9, 00, 00),
                //    DataInicio = new DateTime(2016, 9, 5, 8, 00, 00),
                //    DataFim = new DateTime(2016, 9, 30, 18, 00, 00),
                //    TimeId = 1
                //}
            );
            #endregion

            #region Sprint Planning Meeting Generator
            context.Reunioes.AddOrUpdate(x => x.Id,
                new Reuniao()
                {
                    Id = 1,
                    TipoReuniao = TipoReuniao.Planejamento,
                    DataInicio = new DateTime(2016, 6, 3, 8, 30, 00),
                    DataFim = new DateTime(2016, 6, 3, 12, 00, 00),
                    Local = "Sala Principal",
                    Observacao = "1ª parte da reunião de planejamento",
                    SprintId = 1
                },
                new Reuniao()
                {
                    Id = 1,
                    TipoReuniao = TipoReuniao.Planejamento,
                    DataInicio = new DateTime(2016, 6, 3, 13, 30, 00),
                    DataFim = new DateTime(2016, 6, 3, 18, 00, 00),
                    Local = "Sala 2",
                    Observacao = "2ª parte da reunião de planejamento",
                    SprintId = 1
                }
            );

            context.SprintBacklogs.AddOrUpdate(x => x.Id,
                new SprintBacklog() { Id = 1, ProductBacklogId = 1, SprintId = 1 },
                new SprintBacklog() { Id = 2, ProductBacklogId = 3, SprintId = 1 }
            );
            #endregion

            #region Daily Scrum Generator
            var DataInicialSprint = new DateTime(2016, 6, 6, 8, 00, 00);
            var DaliesScrum = new List<Reuniao>();

            for (int i = 0; i < 25; i++)
            {
                var reuniao = new Reuniao()
                {
                    Id = i + 1,
                    TipoReuniao = TipoReuniao.Diaria,
                    DataInicio = DataInicialSprint.AddDays(i),
                    DataFim = DataInicialSprint.AddDays(i).AddMinutes(15),
                    Local = "Sala Desenvolvimento",
                    Observacao = string.Format("Reunião diária de número {0} com os membros da equipe.", i + 1),
                    SprintId = 1
                };

                DaliesScrum.Add(reuniao);
            }

            context.Reunioes.AddOrUpdate(DaliesScrum.ToArray());
            #endregion

            #region Sprint Review Meeting Generator
            context.Reunioes.AddOrUpdate(x => x.Id,
                new Reuniao()
                {
                    Id = 1,
                    TipoReuniao = TipoReuniao.Revisao,
                    DataInicio = new DateTime(2016, 7, 4, 8, 30, 00),
                    DataFim = new DateTime(2016, 7, 4, 12, 00, 00),
                    Local = "Sala Principal",
                    Observacao = "Todos os envolvidos e comprometidos no projeto estavam presentes.",
                    SprintId = 1
                }
            );
            #endregion

            #region Sprint Retrospective Meeting Generator
            context.Reunioes.AddOrUpdate(x => x.Id,
                new Reuniao()
                {
                    Id = 1,
                    TipoReuniao = TipoReuniao.Retrospectiva,
                    DataInicio = new DateTime(2016, 7, 4, 13, 30, 00),
                    DataFim = new DateTime(2016, 7, 4, 18, 00, 00),
                    Local = "Sala 2",
                    Observacao = "Para o próximo sprint verificar a possibilidade de comprar a licença do DevExpress para auxilio nos gráficos.",
                    SprintId = 1
                }
            );
            #endregion
            #endregion

            #region MembroTime
            context.Tarefas.AddOrUpdate(x => x.Id,
                new Tarefa() { Id = 1, DataInclusao = new DateTime(2016, 6, 3, 15, 0, 0), Descricao = "Criar componente de login reposivo.", MembroTimeId = 1, SprintBacklogId = 1 },
                new Tarefa() { Id = 2, DataInclusao = new DateTime(2016, 6, 3, 15, 30, 0), Descricao = "Criar componente para demonstrar o perfil e a opção de logout.", MembroTimeId = 1, SprintBacklogId = 2 },
                new Tarefa() { Id = 3, DataInclusao = new DateTime(2016, 6, 6, 9, 0, 0), Descricao = "Criar componente para recuperação de senha.", MembroTimeId = 2, SprintBacklogId = 1 }
            );

            context.StatusTarefas.AddOrUpdate(x => x.Id,
                new StatusTarefa() { Id = 1, DataInclusao = new DateTime(2016, 6, 6, 8, 30, 0), MembroTimeId = 1, StatusId = 1, TarefaId = 1 },
                new StatusTarefa() { Id = 2, DataInclusao = new DateTime(2016, 6, 6, 9, 0, 0), MembroTimeId = 1, StatusId = 2, TarefaId = 1 },
                new StatusTarefa() { Id = 3, DataInclusao = new DateTime(2016, 6, 13, 8, 45, 0), MembroTimeId = 1, StatusId = 3, TarefaId = 1 },
                new StatusTarefa() { Id = 4, DataInclusao = new DateTime(2016, 6, 6, 8, 45, 0), MembroTimeId = 2, StatusId = 1, TarefaId = 2 },
                new StatusTarefa() { Id = 5, DataInclusao = new DateTime(2016, 6, 6, 9, 30, 0), MembroTimeId = 2, StatusId = 2, TarefaId = 2 },
                new StatusTarefa() { Id = 6, DataInclusao = new DateTime(2016, 6, 10, 18, 0, 0), MembroTimeId = 2, StatusId = 3, TarefaId = 2 },
                new StatusTarefa() { Id = 7, DataInclusao = new DateTime(2016, 6, 13, 9, 0, 0), MembroTimeId = 2, StatusId = 1, TarefaId = 3 },
                new StatusTarefa() { Id = 8, DataInclusao = new DateTime(2016, 6, 13, 10, 0, 0), MembroTimeId = 2, StatusId = 2, TarefaId = 3 },
                new StatusTarefa() { Id = 9, DataInclusao = new DateTime(2016, 6, 30, 17, 30, 0), MembroTimeId = 2, StatusId = 3, TarefaId = 3 }
            );
            #endregion
        }
    }
}
