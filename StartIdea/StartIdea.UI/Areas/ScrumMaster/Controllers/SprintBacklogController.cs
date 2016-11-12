using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ScrumMaster.Models;
using StartIdea.UI.Areas.ScrumMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ScrumMaster.Controllers
{
    public class SprintBacklogController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public SprintBacklogController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(int? paginaProductBacklog,
                                  int? paginaSprintBacklog,
                                  int? id)
        {
            int paginaProductBacklogNumber = (paginaProductBacklog ?? 1);
            int paginaSprintBacklogNumber = (paginaSprintBacklog ?? 1);

            var sprintBacklogVM = new SprintBacklogVM();
            sprintBacklogVM.SprintId = GetSprintAtual().Id;
            sprintBacklogVM.PaginaGridProductBacklog = paginaProductBacklogNumber;
            sprintBacklogVM.ProductBacklogList = GetGridDataSourceProductBacklog(paginaProductBacklogNumber);
            sprintBacklogVM.PaginaGridSprintBacklog = paginaSprintBacklogNumber;
            sprintBacklogVM.SprintBacklogList = GetGridPagedDataSourceSprintBacklog(paginaSprintBacklogNumber);

            if (id != null)
            {
                sprintBacklogVM.Id = Convert.ToInt32(id);
                sprintBacklogVM.DisplayMotivoCancelamento = "Show";
            }

            return View(sprintBacklogVM);
        }

        public ActionResult Report()
        {
            Sprint sprint = GetSprintAtual();

            var sprintBacklogVM = new SprintBacklogVM();
            sprintBacklogVM.SprintId = sprint.Id;
            sprintBacklogVM.SprintBacklogReport = GetGridDataSourceSprintBacklog();
            ViewBag.ObjetivoSprint = sprint.Objetivo;

            return View(sprintBacklogVM);
        }

        public ActionResult Adicionar(int id, int? paginaProductBacklog, int? paginaSprintBacklog)
        {
            var sprintBacklog = new SprintBacklog()
            {
                ProductBacklogId = id,
                SprintId = GetSprintAtual().Id
            };

            _dbContext.SprintBacklogs.Add(sprintBacklog);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", new
            {
                paginaProductBacklog = paginaProductBacklog,
                paginaSprintBacklog = paginaSprintBacklog
            });
        }

        public ActionResult Remover(SprintBacklogVM sprintBacklogVM, int? paginaProductBacklog, int? paginaSprintBacklog)
        {
            int SprintAtualId = GetSprintAtual().Id;

            #region Cancelar SprintBacklog
            SprintBacklog sprintBacklog = _dbContext.SprintBacklogs.Find(sprintBacklogVM.Id);
            sprintBacklog.MotivoCancelamento = sprintBacklogVM.MotivoCancelamento;
            sprintBacklog.DataCancelamento = DateTime.Now;

            _dbContext.Entry(sprintBacklog).State = EntityState.Modified;
            _dbContext.SaveChanges();
            #endregion

            #region Cancelar Tarefa
            CancelarTarefa(sprintBacklog.Id);
            #endregion

            #region Copiando ProductBacklog
            ProductBacklog productBacklogAntigo = _dbContext.ProductBacklogs.Find(sprintBacklog.ProductBacklogId);
            var productBacklogNovo = new ProductBacklog()
            {
                UserStory = productBacklogAntigo.UserStory,
                Prioridade = 0,
                ProductOwnerId = productBacklogAntigo.ProductOwnerId
            };

            _dbContext.ProductBacklogs.Add(productBacklogNovo);
            _dbContext.SaveChanges();
            #endregion

            return RedirectToAction("Index", new
            {
                paginaProductBacklog = paginaProductBacklog,
                paginaSprintBacklog = paginaSprintBacklog
            });
        }

        private void CancelarTarefa(int SprintBacklogId)
        {
            _dbContext.Tarefas.Where(t => !t.DataCancelamento.HasValue
                                       && t.SprintBacklog.Id == SprintBacklogId).ToList().ForEach(t =>
                                       {
                                           t.DataCancelamento = DateTime.Now;
                                           t.MotivoCancelamento = "Backlog da Sprint cancelada.";
                                       });
            _dbContext.SaveChanges();
        }

        private IPagedList<ProductBacklog> GetGridDataSourceProductBacklog(int PaginaGrid)
        {
            IEnumerable<ProductBacklog> listBacklog = from pb in _dbContext.ProductBacklogs
                                                      where !(from sb in _dbContext.SprintBacklogs
                                                              select sb.ProductBacklogId).Contains(pb.Id)
                                                         && pb.StoryPoint != StoryPoint.N
                                                      orderby pb.Prioridade
                                                      select pb;

            return listBacklog.ToList().ToPagedList(PaginaGrid, 5);
        }

        private IPagedList<SprintBacklog> GetGridPagedDataSourceSprintBacklog(int PaginaGrid)
        {
            return GetGridDataSourceSprintBacklog().ToList().ToPagedList(PaginaGrid, 5);
        }

        private IEnumerable<SprintBacklog> GetGridDataSourceSprintBacklog()
        {
            int SprintAtualId = GetSprintAtual().Id;

            IEnumerable<SprintBacklog> listBacklog = _dbContext.SprintBacklogs.Include(s => s.ProductBacklog)
                                                                              .Where(s => s.SprintId == SprintAtualId
                                                                                       && !s.DataCancelamento.HasValue)
                                                                              .OrderBy(s => s.ProductBacklog.Prioridade)
                                                                              .AsEnumerable();

            return listBacklog;
        }

        private Sprint GetSprintAtual()
        {
            return _dbContext.Sprints.FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                       && s.TimeId == CurrentUser.TimeId
                                                       && s.DataInicial <= DateTime.Now
                                                       && s.DataFinal >= DateTime.Now) ?? new Sprint();
        }
    }
}