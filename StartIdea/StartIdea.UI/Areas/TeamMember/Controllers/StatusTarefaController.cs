using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.TeamMember.Models;
using StartIdea.UI.Areas.TeamMember.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

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
            statusTarefaVM.SprintId = GetSprintId();
            statusTarefaVM.StatusProcesso = _dbContext.AllStatus;
            statusTarefaVM.Tarefas = GetKanbanDataSource();

            return View(statusTarefaVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AlteraStatus(int IdStatus, int IdTarefa)
        {
            Status novoStatus = _dbContext.AllStatus.Find(IdStatus);
            Tarefa tarefa = _dbContext.Tarefas.Include("StatusTarefas.Status").Where(t => t.Id == IdTarefa).FirstOrDefault();

            Status statusAnterior = tarefa.StatusTarefas.OrderByDescending(st => st.DataInclusao).FirstOrDefault().Status;

            if (novoStatus.Classificacao > statusAnterior.Classificacao + 1 ||
                novoStatus.Classificacao < statusAnterior.Classificacao - 1)
            {
                return Json(new { sucesso = false, mensagem = string.Format("Não é permitido alterar a tarefa do status {0} para o status {1}", statusAnterior.Descricao, novoStatus.Descricao) }, JsonRequestBehavior.AllowGet);
            }

            var statusTarefa = new StatusTarefa()
            {
                StatusId = IdStatus,
                TarefaId = IdTarefa,
                MembroTimeId = CurrentUser.PerfilId,
            };

            _dbContext.StatusTarefas.Add(statusTarefa);
            _dbContext.SaveChanges();

            return Json(new { sucesso = true }, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<Tarefa> GetKanbanDataSource()
        {
            int SprintAtualId = GetSprintId();

            return from tarefa in _dbContext.Tarefas.Include("StatusTarefas.Status")
                   where (from sb in _dbContext.SprintBacklogs
                          where !sb.DataCancelamento.HasValue
                             && sb.SprintId == SprintAtualId
                          select sb.Id).Contains(tarefa.SprintBacklogId)
                      && ((from st in _dbContext.StatusTarefas
                           join status in _dbContext.AllStatus
                           on st.StatusId equals status.Id
                           where st.TarefaId == tarefa.Id
                           orderby st.DataInclusao descending
                           select status.Classificacao).Take(1).Contains(Classificacao.Available)
                          ||
                          (from st in _dbContext.StatusTarefas
                           join status in _dbContext.AllStatus
                           on st.StatusId equals status.Id
                           where status.Classificacao != Classificacao.Available
                              && st.MembroTimeId == CurrentUser.PerfilId
                              && st.DataInclusao == _dbContext.StatusTarefas.Where(st => st.TarefaId == tarefa.Id)
                                                                            .OrderByDescending(st => st.DataInclusao)
                                                                            .FirstOrDefault().DataInclusao
                           select st.TarefaId).Contains(tarefa.Id))
                      && !tarefa.DataCancelamento.HasValue
                   select tarefa;
        }

        private int GetSprintId()
        {
            var sprint = _dbContext.Sprints.FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                              && s.TimeId == CurrentUser.TimeId
                                                              && s.DataInicial <= DateTime.Now
                                                              && s.DataFinal >= DateTime.Now) ?? new Sprint();

            return sprint.Id;
        }
    }
}