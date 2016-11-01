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
        private StartIdeaDBContext _dbContext;

        public SprintController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

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

                _dbContext.Sprints.Add(sprint);
                _dbContext.SaveChanges();

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
                var sprintAux = _dbContext.Sprints.Find(sprintVM.Id);
                sprintAux.Objetivo = sprintVM.Objetivo;
                sprintAux.DataInicial = sprintVM.DataInicial;
                sprintAux.DataFinal = sprintVM.DataFinal;

                _dbContext.Entry(sprintAux).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }

            return View("Index", sprintVM);
        }
    }
}
