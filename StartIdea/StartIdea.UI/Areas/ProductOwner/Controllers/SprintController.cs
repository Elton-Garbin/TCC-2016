using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ProductOwner.Models;
using StartIdea.UI.Areas.ProductOwner.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ProductOwner.Controllers
{
    public class SprintController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public SprintController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            var sprint = GetSprintAtual();
            var sprintVM = new SprintVM();
            sprintVM.Id = sprint.Id;
            sprintVM.Objetivo = sprint.Objetivo;
            sprintVM.DataInicial = sprint.DataInicial;
            sprintVM.DataFinal = sprint.DataFinal;
            sprintVM.DataCadastro = sprint.DataCadastro;

            return View(sprintVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MotivoCancelamento")] SprintVM sprintVM)
        {
            if (ModelState.IsValid)
            {
                Sprint sprint = _dbContext.Sprints.Find(sprintVM.Id);
                sprint.DataCancelamento = DateTime.Now;
                sprint.MotivoCancelamento = sprintVM.MotivoCancelamento;

                _dbContext.Entry(sprint).State = EntityState.Modified;
                _dbContext.SaveChanges();

                CadastrarProductBacklog(sprint.Id);
                CancelarSprintBacklog(sprint.Id);
                CancelarTarefa(sprint.Id);

                return RedirectToAction("Index");
            }

            return View("Index", sprintVM);
        }

        private void CancelarTarefa(int SprintId)
        {
            _dbContext.Tarefas.Include(t => t.SprintBacklog)
                              .Where(t => !t.DataCancelamento.HasValue
                                       && t.SprintBacklog.SprintId == SprintId).ToList().ForEach(t =>
            {
                t.DataCancelamento = DateTime.Now;
                t.MotivoCancelamento = "Sprint cancelada.";
            });
            _dbContext.SaveChanges();
        }

        private void CancelarSprintBacklog(int SprintId)
        {
            _dbContext.SprintBacklogs.Where(s => s.SprintId == SprintId
                                              && !s.DataCancelamento.HasValue).ToList().ForEach(s =>
            {
                s.DataCancelamento = DateTime.Now;
                s.MotivoCancelamento = "Sprint cancelada.";
            });
            _dbContext.SaveChanges();
        }

        private void CadastrarProductBacklog(int SprintId)
        {
            IEnumerable<ProductBacklog> ProductBacklogList = (from pb in _dbContext.ProductBacklogs
                                                              where (from sb in _dbContext.SprintBacklogs
                                                                     where !sb.DataCancelamento.HasValue
                                                                        && sb.SprintId == SprintId
                                                                     select sb.ProductBacklogId).Contains(pb.Id)
                                                              select new
                                                              {
                                                                  UserStory = pb.UserStory,
                                                              }).AsEnumerable().Select(x => new ProductBacklog()
                                                              {
                                                                  UserStory = x.UserStory,
                                                                  ProductOwnerId = CurrentUser.PerfilId,
                                                                  Prioridade = 0
                                                              });

            _dbContext.ProductBacklogs.AddRange(ProductBacklogList);
            _dbContext.SaveChanges();
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
