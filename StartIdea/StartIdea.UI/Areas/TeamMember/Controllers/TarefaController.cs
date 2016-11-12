using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.TeamMember.Models;
using StartIdea.UI.Areas.TeamMember.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.TeamMember.Controllers
{
    public class TarefaController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public TarefaController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(string FiltroDescricao,
                                  int? PaginaGrid,
                                  int? IdCancelamento)
        {
            var tarefaVM = new TarefaVM();
            tarefaVM.FiltroDescricao = FiltroDescricao;
            tarefaVM.PaginaGrid = (PaginaGrid ?? 1);
            tarefaVM.SprintId = GetSprintId();

            if ((IdCancelamento ?? 0) > 0)
            {
                Tarefa tarefa = _dbContext.Tarefas.Find(IdCancelamento);
                if (tarefa == null)
                    return HttpNotFound();

                tarefaVM.TarefaIdCancelamento = tarefa.Id;
                tarefaVM.DisplayMotivoCancelamento = "Show";
            }

            tarefaVM.TarefaList = GetGridDataSource(tarefaVM);
            tarefaVM.SprintBacklogs = GetSprintsBacklog();

            return View(tarefaVM);
        }

        public ActionResult Create()
        {
            return View(new TarefaVM()
            {
                SprintBacklogs = GetSprintsBacklog()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "TarefaList,PaginaGrid,SprintBacklogs,Id")] TarefaVM tarefaVM)
        {
            if (ModelState.IsValid)
            {
                Tarefa tarefa = new Tarefa()
                {
                    Descricao = tarefaVM.Descricao,
                    SprintBacklogId = tarefaVM.SprintBacklogId,
                    MembroTimeId = CurrentUser.PerfilId
                };

                StatusTarefa statusTarefa = new StatusTarefa()
                {
                    MembroTimeId = CurrentUser.PerfilId,
                    Tarefa = tarefa,
                    StatusId = _dbContext.AllStatus.Where(s => s.Classificacao == Classificacao.Available).SingleOrDefault().Id
                };

                _dbContext.StatusTarefas.Add(statusTarefa);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            tarefaVM.SprintBacklogs = GetSprintsBacklog();
            return View(tarefaVM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tarefa tarefa = _dbContext.Tarefas.Find(id);
            if (tarefa == null)
                return HttpNotFound();

            var tarefaVM = new TarefaVM()
            {
                Id = tarefa.Id,
                Descricao = tarefa.Descricao,
                SprintBacklogId = tarefa.SprintBacklogId,
                SprintBacklogs = GetSprintsBacklog()
            };

            return View(tarefaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "TarefaList,PaginaGrid,SprintBacklogs")] TarefaVM tarefaVM)
        {
            if (ModelState.IsValid)
            {
                Tarefa tarefa = _dbContext.Tarefas.Find(tarefaVM.Id);
                tarefa.Descricao = tarefaVM.Descricao;
                tarefa.SprintBacklogId = tarefaVM.SprintBacklogId;

                _dbContext.Entry(tarefa).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            tarefaVM.SprintBacklogs = GetSprintsBacklog();
            return View(tarefaVM);
        }

        public ActionResult Report(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tarefa = _dbContext.Tarefas.Include(t => t.SprintBacklog.Sprint)
                                           .Include(t => t.SprintBacklog.ProductBacklog)
                                           .Include(t => t.MembroTime.Usuario)
                                           .Where(t => t.Id == id)
                                           .FirstOrDefault();
            if (tarefa == null)
                return HttpNotFound();

            return View(tarefa);
        }

        public ActionResult Cancel(TarefaVM tarefaVM)
        {
            if (tarefaVM.TarefaIdCancelamento == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tarefa tarefa = _dbContext.Tarefas.Find(tarefaVM.TarefaIdCancelamento);
            if (tarefa == null)
                return HttpNotFound();

            tarefa.MotivoCancelamento = tarefaVM.MotivoCancelamento;
            tarefa.DataCancelamento = DateTime.Now;

            _dbContext.Entry(tarefa).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private IPagedList<Tarefa> GetGridDataSource(TarefaVM tarefaVM)
        {
            IEnumerable<Tarefa> listTarefas = from t in _dbContext.Tarefas
                                              join sb in _dbContext.SprintBacklogs
                                              on t.SprintBacklogId equals sb.Id
                                              join pb in _dbContext.ProductBacklogs
                                              on sb.ProductBacklogId equals pb.Id
                                              where sb.SprintId == tarefaVM.SprintId
                                                 && (from st in _dbContext.StatusTarefas
                                                     join status in _dbContext.AllStatus
                                                     on st.StatusId equals status.Id
                                                     where st.TarefaId == t.Id
                                                     orderby st.DataInclusao descending
                                                     select status.Classificacao).Take(1).Contains(Classificacao.Available)
                                                 && !sb.DataCancelamento.HasValue
                                                 && !t.DataCancelamento.HasValue
                                              orderby pb.Prioridade
                                              select t;

            if (!string.IsNullOrEmpty(tarefaVM.FiltroDescricao))
                listTarefas = listTarefas.Where(t => t.Descricao.ToUpper().Contains(tarefaVM.FiltroDescricao.ToUpper()));

            return listTarefas.ToList().ToPagedList(tarefaVM.PaginaGrid, 7);
        }

        private IEnumerable<SprintBacklog> GetSprintsBacklog()
        {
            int SprintAtualId = GetSprintId();

            return from sb in _dbContext.SprintBacklogs.Include("ProductBacklog")
                   where !sb.DataCancelamento.HasValue
                      && sb.SprintId == SprintAtualId
                   orderby sb.ProductBacklog.Prioridade
                   select sb;
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