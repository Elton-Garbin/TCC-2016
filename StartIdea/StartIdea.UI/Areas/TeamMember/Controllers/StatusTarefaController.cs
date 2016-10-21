using System.Web.Mvc;
using StartIdea.UI.Areas.TeamMember.Models;
using StartIdea.DataAccess;
using StartIdea.UI.Areas.TeamMember.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using StartIdea.Model.ScrumArtefatos;
using Newtonsoft.Json;

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
            statusTarefaVM.StatusProcesso = _dbContext.AllStatus.OrderBy(s => s.Classificacao);

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

            statusTarefaVM.TarefasProcesso = from tarefa in _dbContext.Tarefas.Include("StatusTarefas.Status")
                                             where (from sb in _dbContext.SprintBacklogs
                                                    where !sb.DataCancelamento.HasValue
                                                    && (from sprint in _dbContext.Sprints
                                                        where !sprint.DataCancelamento.HasValue
                                                        && sprint.DataInicial <= DateTime.Now
                                                        && sprint.DataFinal >= DateTime.Now
                                                        select sprint.Id).Contains(sb.SprintId)
                                                    select sb.Id).Contains(tarefa.SprintBacklogId)
                                             && (from st in _dbContext.StatusTarefas
                                                  select st.TarefaId).Contains(tarefa.Id)
                                             select tarefa;

            return View(statusTarefaVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AlteraStatus(int IdStatus, int IdTarefa)
        {
            var status = _dbContext.AllStatus.Find(IdStatus);
            var statusTarefaAnterior = _dbContext.StatusTarefas.Include("Status").Where(st => st.TarefaId == IdTarefa).OrderByDescending(st => st.DataInclusao).First();

            if (statusTarefaAnterior.Status.Classificacao > status.Classificacao + 1 ||
                statusTarefaAnterior.Status.Classificacao < status.Classificacao - 1)
            {
                return Json(new { sucesso = false }, JsonRequestBehavior.AllowGet);
            }

            StatusTarefa statusTarefaInclusao = new StatusTarefa()
            {
                StatusId = IdStatus,
                TarefaId = IdTarefa,
                MembroTimeId = CurrentUser.TimeId
            };

            _dbContext.StatusTarefas.Add(statusTarefaInclusao);
            _dbContext.SaveChanges();

            return Json(new { sucesso = true }, JsonRequestBehavior.AllowGet);
        }
    }
}