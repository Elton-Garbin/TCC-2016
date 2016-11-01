using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    [AllowAnonymous]
    public class TeamKanbanBoardController : Controller
    {
        private StartIdeaDBContext _dbContext;

        public TeamKanbanBoardController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            var teamKanbanBoardVM = new TeamKanbanBoardVM();
            teamKanbanBoardVM.StatusProcesso = _dbContext.AllStatus;

            teamKanbanBoardVM.Tarefas = from tarefa in _dbContext.Tarefas.Include("StatusTarefas.Status").Include("StatusTarefas.MembroTime.Usuario")
                                        where (from sb in _dbContext.SprintBacklogs
                                               where !sb.DataCancelamento.HasValue
                                               && (from sprint in _dbContext.Sprints
                                                   where !sprint.DataCancelamento.HasValue
                                                   && sprint.DataInicial <= DateTime.Now
                                                   && sprint.DataFinal >= DateTime.Now
                                                   select sprint.Id).Contains(sb.SprintId)
                                               select sb.Id).Contains(tarefa.SprintBacklogId)
                                        && ((from st in _dbContext.StatusTarefas
                                             join status in _dbContext.AllStatus
                                             on st.StatusId equals status.Id
                                             where st.TarefaId == tarefa.Id
                                             orderby st.DataInclusao descending
                                             select status.Classificacao).Take(1).Contains(Classificacao.Available) ||
                                             (from st in _dbContext.StatusTarefas
                                              join status in _dbContext.AllStatus
                                              on st.StatusId equals status.Id
                                              where status.Classificacao != Classificacao.Available
                                              && st.DataInclusao == _dbContext.StatusTarefas.Where(st => st.TarefaId == tarefa.Id).OrderByDescending(st => st.DataInclusao).FirstOrDefault().DataInclusao
                                              select st.TarefaId).Contains(tarefa.Id))
                                        && !tarefa.DataCancelamento.HasValue
                                        select tarefa;

            return View(teamKanbanBoardVM);
        }
    }
}