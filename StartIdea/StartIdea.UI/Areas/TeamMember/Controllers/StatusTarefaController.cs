using System.Web.Mvc;
using StartIdea.UI.Areas.TeamMember.Models;
using StartIdea.DataAccess;
using StartIdea.UI.Areas.TeamMember.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;

namespace StartIdea.UI.Areas.TeamMember.Controllers
{
    public class StatusTarefaController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public StatusTarefaController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            var statusTarefaVM = new StatusTarefaVM();
            statusTarefaVM.StatusProcesso = _dbContext.AllStatus;

            statusTarefaVM.TarefasPendentes = from tarefa in _dbContext.Tarefas
                                              where (from sb in _dbContext.SprintBacklogs
                                                     where !sb.DataCancelamento.HasValue
                                                     && (from sprint in _dbContext.Sprints
                                                         where !sprint.DataCancelamento.HasValue
                                                         && sprint.DataInicial <= DateTime.Now
                                                         && sprint.DataFinal >= DateTime.Now
                                                         select sprint.Id).Contains(sb.SprintId)
                                                     select sb.Id).Contains(tarefa.SprintBacklogId)
                                              && !(from st in _dbContext.StatusTarefas
                                                   select st.TarefaId).Contains(tarefa.Id)
                                              select tarefa;



            return View(statusTarefaVM);
        }
    }
}