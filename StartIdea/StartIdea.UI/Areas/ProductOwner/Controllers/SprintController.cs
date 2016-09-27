using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ProductOwner.ViewModels;
using System;
using System.Data.Entity;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ProductOwner.Controllers
{
    public class SprintController : Controller
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public ActionResult Index()
        {
            return View(new SprintVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Id,DataInicial,DataFinal,Objetivo,DataCadastro,DataCancelamento")] SprintVM sprintVM)
        {
            if (ModelState.IsValid)
            {
                Sprint sprint = sprintVM.GetSprintAtual();
                sprint.DataCancelamento = DateTime.Now;
                sprint.MotivoCancelamento = sprintVM.MotivoCancelamento;

                dbContext.Entry(sprint).State = EntityState.Modified;
                dbContext.SaveChanges();

                return RedirectToAction("Index");
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
