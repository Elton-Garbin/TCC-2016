using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ScrumMaster.Models;
using StartIdea.UI.Areas.ScrumMaster.ViewModels;
using System.Data.Entity;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ScrumMaster.Controllers
{
    public class SprintController : AppController
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public ActionResult Index()
        {
            return View(new SprintVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SprintVM sprintVM)
        {
            if (ModelState.IsValid)
            {
                var sprint = new Sprint()
                {
                    Objetivo = sprintVM.Objetivo,
                    DataInicial = sprintVM.DataInicial,
                    DataFinal = sprintVM.DataFinal,
                    TimeId = sprintVM.TimeId
                };

                dbContext.Sprints.Add(sprint);
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Index", sprintVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SprintVM sprintVM)
        {
            if (ModelState.IsValid)
            {
                var sprintAux = dbContext.Sprints.Find(sprintVM.Id);
                sprintAux.Objetivo = sprintVM.Objetivo;
                sprintAux.DataInicial = sprintVM.DataInicial;
                sprintAux.DataFinal = sprintVM.DataFinal;

                dbContext.Entry(sprintAux).State = EntityState.Modified;
                dbContext.SaveChanges();
            }

            return View("Index", sprintVM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}
