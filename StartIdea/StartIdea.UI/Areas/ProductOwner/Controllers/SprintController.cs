using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ProductOwner.Models;
using StartIdea.UI.Areas.ProductOwner.ViewModels;
using System;
using System.Data.Entity;
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
            return View(new SprintVM(CurrentUser.TimeId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DataCancelamento,MotivoCancelamento")] SprintVM sprintVM)
        {
            if (ModelState.IsValid)
            {
                Sprint sprint = sprintVM.GetSprintAtual();
                sprint.DataCancelamento = DateTime.Now;
                sprint.MotivoCancelamento = sprintVM.MotivoCancelamento;

                _dbContext.Entry(sprint).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Index", sprintVM);
        }
    }
}
