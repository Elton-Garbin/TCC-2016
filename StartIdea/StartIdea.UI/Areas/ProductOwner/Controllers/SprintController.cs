using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ProductOwner.Models;
using StartIdea.UI.Areas.ProductOwner.ViewModels;
using System;
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

                return RedirectToAction("Index");
            }

            return View("Index", sprintVM);
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
