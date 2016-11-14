using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Models;
using StartIdea.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    [AllowAnonymous]
    public class BurndownChartController : Controller
    {
        private StartIdeaDBContext _dbContext;

        public BurndownChartController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(int? id)
        {
            var vm = new BurndownChartVM();
            vm.SprintDesejada = (id == null) ? GetSprintAtual() : GetSprint(Convert.ToInt32(id));
            vm.IsActualSprint = (id == null);

            if (vm.SprintDesejada.Id > 0)
            {
                vm.Labels = GetLabels(vm.SprintDesejada);
                vm.Datasets = GetDatasets(vm.SprintDesejada);
            }

            return View(vm);
        }

        private List<ChartDatasetFacade> GetDatasets(Sprint sprint)
        {
            var dataSets = new List<ChartDatasetFacade>();
            int TotalPoints = GetPoints(sprint.Id);

            dataSets.Add(new ChartDatasetFacade("rgba(255, 0, 0, 0.3)")
            {
                label = "Atual",
                data = GetActualData(sprint, TotalPoints)
            });
            dataSets.Add(new ChartDatasetFacade("rgba(0, 0, 255, 0.3)")
            {
                label = "Planejado",
                data = GetPlanData(sprint, TotalPoints),
                borderDash = new short[] { 5, 5 }
            });
            dataSets.Add(new ChartDatasetFacade("rgba(0, 255, 0, 0.3)")
            {
                label = "Velocidade",
                data = GetVelocityData(sprint),
                borderDash = new short[] { 5, 5 }
            });

            return dataSets;
        }

        private double[] GetVelocityData(Sprint sprint)
        {
            var velocityData = new List<double>();
            var lstDailyScrum = sprint.Reunioes.Where(r => r.TipoReuniao == TipoReuniao.Diaria)
                                               .OrderBy(r => r.DataInicial);

            foreach (var dailyScrum in lstDailyScrum)
            {
                var dataBase = dailyScrum.DataInicial.Date;
                if (dataBase >= DateTime.Today)
                    break;

                double points = GetPoints(sprint.Id, dataBase, GetPointsMode.SomenteDateBase);
                velocityData.Add(points);
            }

            return velocityData.ToArray();
        }

        private double[] GetPlanData(Sprint sprint, int TotalPoints)
        {
            var planData = new List<double>();

            int qtDias = sprint.Reunioes.Where(r => r.TipoReuniao == TipoReuniao.Diaria).Count();
            double points = TotalPoints;
            double media = points / qtDias;

            for (int i = qtDias; i > 0; i--)
            {
                planData.Add(points);
                points = points - media;
            }

            return planData.ToArray();
        }

        private double[] GetActualData(Sprint sprint, int TotalPoints)
        {
            var actualData = new List<double>();
            var lstDailyScrum = sprint.Reunioes.Where(r => r.TipoReuniao == TipoReuniao.Diaria)
                                               .OrderBy(r => r.DataInicial);

            foreach (var dailyScrum in lstDailyScrum)
            {
                var dataBase = dailyScrum.DataInicial.Date;
                if (dataBase >= DateTime.Today)
                    break;

                double points = TotalPoints - GetPoints(sprint.Id, dataBase, GetPointsMode.AteDataBase);
                actualData.Add(points);
            }

            return actualData.ToArray();
        }

        private int GetPoints(int SprintId)
        {
            var query = from pb in _dbContext.ProductBacklogs
                        where (from sb in _dbContext.SprintBacklogs
                               where sb.SprintId.Equals(SprintId)
                                  && !sb.DataCancelamento.HasValue
                                  && (from t in _dbContext.Tarefas
                                      where !sb.DataCancelamento.HasValue
                                      select t.SprintBacklogId).Contains(sb.Id)
                               select sb.ProductBacklogId).Contains(pb.Id)
                        select pb.StoryPoint;

            if (Utils.IsEmpty(query.AsEnumerable()))
                return 0;

            var points = query.Sum(p => (int)p);
            return Convert.ToInt32(points);
        }

        private enum GetPointsMode
        {
            AteDataBase,
            SomenteDateBase
        }
        private double GetPoints(int SprintId, DateTime DataBase, GetPointsMode Mode)
        {
            var subQuery = from st in _dbContext.StatusTarefas
                           group st by st.TarefaId into grouping
                           select new
                           {
                               TarefaId = grouping.Key,
                               MaxStatusTarefaId = grouping.Max(x => x.Id)
                           };

            var query = from t in _dbContext.Tarefas
                        join st in _dbContext.StatusTarefas
                        on t.Id equals st.TarefaId
                        join s in _dbContext.AllStatus
                        on st.StatusId equals s.Id
                        join sb in _dbContext.SprintBacklogs
                        on t.SprintBacklogId equals sb.Id
                        join pb in _dbContext.ProductBacklogs
                        on sb.ProductBacklogId equals pb.Id
                        join sq in subQuery
                        on st.Id equals sq.MaxStatusTarefaId
                        where !t.DataCancelamento.HasValue
                           && !sb.DataCancelamento.HasValue
                           && sb.SprintId == SprintId
                        select new
                        {
                            TarefaId = t.Id,
                            ClassificacaoStatus = s.Classificacao,
                            SprintBacklogId = sb.Id,
                            StoryPointBacklog = pb.StoryPoint,
                            DataStatusTarefa = st.DataInclusao
                        };

            double points = 0;
            foreach (var row in query)
            {
                if ((row.ClassificacaoStatus == Classificacao.Done) &&
                    ((Mode == GetPointsMode.AteDataBase && row.DataStatusTarefa.Date <= DataBase) ||
                     (Mode == GetPointsMode.SomenteDateBase && row.DataStatusTarefa.Date == DataBase)))
                {
                    int totalSprintBacklog = query.Count(x => x.SprintBacklogId == row.SprintBacklogId);
                    double media = ((double)row.StoryPointBacklog / totalSprintBacklog);

                    points += media;
                }
            }

            return points;
        }

        private string[] GetLabels(Sprint sprint)
        {
            var dias = new List<string>();

            int total = sprint.Reunioes.Where(r => r.TipoReuniao == TipoReuniao.Diaria).Count();
            for (int i = 1; i <= total; i++)
                dias.Add(i.ToString());

            return dias.ToArray();
        }

        private Sprint GetSprintAtual()
        {
            return _dbContext.Sprints.Include(s => s.Reunioes)
                                     .FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                        && s.TimeId == 1
                                                        && s.DataInicial <= DateTime.Now
                                                        && s.DataFinal >= DateTime.Now) ?? new Sprint();
        }

        private Sprint GetSprint(int SprintId)
        {
            return _dbContext.Sprints.Include(s => s.Reunioes)
                                     .FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                        && s.TimeId == 1
                                                        && s.Id == SprintId) ?? new Sprint();
        }
    }
}