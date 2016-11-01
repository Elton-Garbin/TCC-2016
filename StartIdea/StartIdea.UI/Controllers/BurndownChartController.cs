using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Models.Facade;
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

        public ActionResult Index()
        {
            var vm = new BurndownChartVM();
            vm.SprintAtual = GetSprintAtual();

            if (vm.SprintAtual != null)
            {
                vm.Labels = GetLabels(vm.SprintAtual.DataInicial, vm.SprintAtual.DataFinal);
                vm.Datasets = GetDatasets(vm.SprintAtual);
            }

            return View(vm);
        }

        private List<ChartDataset> GetDatasets(Sprint sprint)
        {
            var dataSets = new List<ChartDataset>();
            int TotalPoints = GetPoints(sprint.Id);

            dataSets.Add(new ChartDataset("rgba(255, 0, 0, 0.3)")
            {
                label = "Atual",
                data = GetActualData(sprint, TotalPoints)
            });
            dataSets.Add(new ChartDataset("rgba(0, 0, 255, 0.3)")
            {
                label = "Planejado",
                data = GetPlanData(sprint, TotalPoints),
                borderDash = new short[] { 5, 5 }
            });

            return dataSets;
        }

        private double[] GetPlanData(Sprint sprint, int TotalPoints)
        {
            var planData = new List<double>();

            int qtDias = (sprint.DataFinal - sprint.DataInicial).Days;
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

            int qtDias = (sprint.DataFinal - sprint.DataInicial).Days;
            for (int i = 0; i < qtDias; i++)
            {
                var dataBase = sprint.DataInicial.Date.AddDays(i);
                if (dataBase == DateTime.Now.Date)
                    break;

                var points = TotalPoints - GetPoints(sprint.Id, dataBase);
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

            if (query == null)
                return 0;

            var points = query.Sum(p => (int)p);
            return Convert.ToInt32(points);
        }

        private double GetPoints(int SprintId, DateTime DataBase)
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
                           && DbFunctions.TruncateTime(st.DataInclusao) < DataBase
                        select new
                        {
                            TarefaId = t.Id,
                            ClassificacaoStatus = s.Classificacao,
                            SprintBacklogId = sb.Id,
                            StoryPointBacklog = pb.StoryPoint
                        };

            double points = 0;
            foreach (var row in query)
            {
                if (row.ClassificacaoStatus == Classificacao.Done)
                {
                    int totalSprintBacklog = query.Count(x => x.SprintBacklogId == row.SprintBacklogId);
                    double media = (int)row.ClassificacaoStatus / totalSprintBacklog;

                    points += media;
                }
            }

            return points;
        }

        private string[] GetLabels(DateTime DataInicial, DateTime DataFinal)
        {
            var dias = new List<string>();

            int total = (DataFinal - DataInicial).Days;
            for (int i = 1; i <= total; i++)
                dias.Add(i.ToString());

            return dias.ToArray();
        }

        private Sprint GetSprintAtual()
        {
            return _dbContext.Sprints.FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                        && s.TimeId == 1
                                                        && s.DataInicial <= DateTime.Now
                                                        && s.DataFinal >= DateTime.Now) ?? new Sprint();
        }
    }
}