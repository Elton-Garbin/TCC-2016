using System.Web.Mvc;
using StartIdea.UI.Areas.TeamMember.Models;
using StartIdea.DataAccess;
using StartIdea.UI.Areas.TeamMember.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using StartIdea.Model.ScrumArtefatos;

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

            statusTarefaVM.Tarefas = from tarefa in _dbContext.Tarefas.Include("StatusTarefas.Status")
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
                                           && st.MembroTimeId == CurrentUser.PerfilId
                                           && st.DataInclusao == _dbContext.StatusTarefas.Where(st => st.TarefaId == tarefa.Id).OrderByDescending(st => st.DataInclusao).FirstOrDefault().DataInclusao
                                           select st.TarefaId).Contains(tarefa.Id))
                                     select tarefa;

            return View(statusTarefaVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AlteraStatus (int IdStatus, int IdTarefa)
        {
            Status novoStatus = _dbContext.AllStatus.Find(IdStatus);
            Tarefa tarefa = _dbContext.Tarefas.Include("StatusTarefas.Status").Where(t => t.Id == IdTarefa).FirstOrDefault();

            Status statusAnterior = tarefa.StatusTarefas.OrderByDescending(st => st.DataInclusao).FirstOrDefault().Status;

            if (novoStatus.Classificacao > statusAnterior.Classificacao + 1 ||
                novoStatus.Classificacao < statusAnterior.Classificacao - 1)
            {
                return Json(new { sucesso = false, mensagem = string.Format("Não é permitido alterar a tarefa do status {0} para o status {1}", statusAnterior.Descricao, novoStatus.Descricao) }, JsonRequestBehavior.AllowGet);
            }

            StatusTarefa statusTarefa = new StatusTarefa()
            {
                StatusId = IdStatus,
                TarefaId = IdTarefa,
                MembroTimeId = CurrentUser.PerfilId,
            };

            _dbContext.StatusTarefas.Add(statusTarefa);
            _dbContext.SaveChanges();

            return Json(new { sucesso = true }, JsonRequestBehavior.AllowGet);
        }
    }
}