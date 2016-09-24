using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ScrumMaster.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ScrumMaster.Controllers
{
    public class SprintController : Controller
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public ActionResult Index()
        {
            var sprintVM = new SprintVM();
            int IdTime = GetScrumMasterTeamId();

            sprintVM.SprintAtual = dbContext.Sprints.SingleOrDefault(s => !s.DataCancelamento.HasValue
                                                                     && s.TimeId == IdTime
                                                                     && s.DataInicial <= DateTime.Now
                                                                     && s.DataFinal >= DateTime.Now);

            sprintVM.ProximaSprint = dbContext.Sprints.Where(s => !s.DataCancelamento.HasValue
                                                             && s.TimeId == IdTime
                                                             && s.DataInicial > DateTime.Now)
                                                      .OrderByDescending(s => s.DataInicial)
                                                      .FirstOrDefault() ?? new Sprint();

            return View(sprintVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SprintVM sprintVM)
        {
            if (ModelState.IsValid)
            {
                var sprint = new Sprint()
                {
                    Objetivo = sprintVM.ProximaSprint.Objetivo,
                    DataInicial = sprintVM.ProximaSprint.DataInicial,
                    DataFinal = sprintVM.ProximaSprint.DataFinal,
                    TimeId = GetScrumMasterTeamId()
                };

                dbContext.Sprints.Add(sprint);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private int GetScrumMasterTeamId()
        {
            var time = dbContext.Times.Where(t => t.ScrumMasterId == 1).FirstOrDefault();

            return time.Id;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SprintVM sprintVM)
        {
            if (ModelState.IsValid)
            {
                var sprintAux = dbContext.Sprints.Find(sprintVM.ProximaSprint.Id);
                sprintAux.Objetivo = sprintVM.ProximaSprint.Objetivo;
                sprintAux.DataInicial = sprintVM.ProximaSprint.DataInicial;
                sprintAux.DataFinal = sprintVM.ProximaSprint.DataFinal;

                dbContext.Entry(sprintAux).State = EntityState.Modified;
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
