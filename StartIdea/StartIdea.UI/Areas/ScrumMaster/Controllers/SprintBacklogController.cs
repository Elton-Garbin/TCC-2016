using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ScrumMaster.Models;
using StartIdea.UI.Areas.ScrumMaster.ViewModels;
using System;
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

        // GET: ScrumMaster/SprintBacklog
        public ActionResult Index(int? paginaProductBacklog,
                                  int? paginaSprintBacklog,
                                  int? id)
        {
            var sprintBacklogVM = new SprintBacklogVM();

            int paginaProductBacklogNumber = (paginaProductBacklog ?? 1);
            int paginaSprintBacklogNumber = (paginaSprintBacklog ?? 1);

            var backlogProduto = from pb in _dbContext.ProductBacklogs
                                 where !(from sb in _dbContext.SprintBacklogs
                                         select sb.ProductBacklogId)
                                         .Contains(pb.Id) &&
                                 pb.StoryPoint != StoryPoint.N
                                 orderby pb.Prioridade
                                 select pb;

            var sprintBacklog = from pb in _dbContext.ProductBacklogs
                                where (from sb in _dbContext.SprintBacklogs
                                       where !sb.DataCancelamento.HasValue
                                       && (from sprint in _dbContext.Sprints
                                           where !sprint.DataCancelamento.HasValue
                                           && sprint.DataInicial <= DateTime.Now
                                           && sprint.DataFinal >= DateTime.Now
                                           select sprint.Id).Contains(sb.SprintId)
                                       select sb.ProductBacklogId).Contains(pb.Id)
                                orderby pb.Prioridade
                                select pb;
                                      
            sprintBacklogVM.paginaProductBacklog = paginaProductBacklogNumber;
            sprintBacklogVM.ProductBacklog = backlogProduto.ToPagedList(paginaProductBacklogNumber, 5);

            sprintBacklogVM.paginaSprintBacklog = paginaSprintBacklogNumber;
            sprintBacklogVM.SprintBacklog = sprintBacklog.ToPagedList(paginaSprintBacklogNumber, 5);

            if (id != null)
            {
                sprintBacklogVM.Id = Convert.ToInt32(id);
                sprintBacklogVM.DisplayMotivoCancelamento = "Show";
            }

            return View(sprintBacklogVM);
        }

        public ActionResult Adicionar(int id, int? paginaProductBacklog, int? paginaSprintBacklog)
        {
            int SprintAtual = GetSprintAtual().Id;

            SprintBacklog sprintBacklog = new SprintBacklog() {
                ProductBacklogId = id,
                SprintId = SprintAtual
            };

            _dbContext.SprintBacklogs.Add(sprintBacklog);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", new {
                paginaProductBacklog = paginaProductBacklog,
                paginaSprintBacklog = paginaSprintBacklog
            });
        }

        public ActionResult Remover(SprintBacklogVM sprintBacklogVM, int? paginaProductBacklog, int? paginaSprintBacklog)
        {
            Sprint sprintAtual = GetSprintAtual();

            SprintBacklog sprintBacklog = _dbContext.SprintBacklogs.Where(sb => sb.ProductBacklogId == sprintBacklogVM.Id
                                                                          && sb.SprintId == sprintAtual.Id
                                                                          && !sb.DataCancelamento.HasValue)
                                                                   .SingleOrDefault();

            sprintBacklog.MotivoCancelamento = sprintBacklogVM.MotivoCancelamento;
            sprintBacklog.DataCancelamento   = DateTime.Now;

            ProductBacklog productBacklogExcluir = _dbContext.ProductBacklogs.Find(sprintBacklogVM.Id);

            ProductBacklog productBacklogIncluir = new ProductBacklog()
            {
                UserStory = productBacklogExcluir.UserStory,
                Prioridade = 0,
                ProductOwnerId = productBacklogExcluir.ProductOwnerId
            };

            _dbContext.ProductBacklogs.Add(productBacklogIncluir);
            _dbContext.Entry(sprintBacklog).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return RedirectToAction("Index", new {
                paginaProductBacklog = paginaProductBacklog,
                paginaSprintBacklog = paginaSprintBacklog
            });
        }

        public Sprint GetSprintAtual()
        {
            return _dbContext.Sprints.FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                     && s.TimeId == CurrentUser.TimeId
                                                     && s.DataInicial <= DateTime.Now
                                                     && s.DataFinal >= DateTime.Now) ?? new Sprint();
        }
    }
}