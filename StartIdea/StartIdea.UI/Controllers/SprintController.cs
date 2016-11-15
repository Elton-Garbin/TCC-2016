using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    [AllowAnonymous]
    public class SprintController : Controller
    {
        private StartIdeaDBContext _dbContext;

        public SprintController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(string contextoBusca, string filtro, DateTime? dataInicial, DateTime? dataFinal, int? pagina)
        {
            var sprintVM = new SprintVM();

            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtro;

            ViewBag.Filtro = contextoBusca;
            ViewBag.DataInicialAtual = dataInicial;
            ViewBag.DataFinalAtual = dataFinal;

            int pageNumber = (pagina ?? 1);
            IEnumerable<Sprint> sprints = null;

            if (!string.IsNullOrEmpty(contextoBusca))
                sprints = _dbContext.Sprints.Where(sprint => sprint.Objetivo.ToUpper().Contains(contextoBusca.ToUpper())).ToList();
            else
                sprints = _dbContext.Sprints.ToList();

            if (dataInicial != null)
                sprints = sprints.Where(sprint => sprint.DataInicial.Date >= ((DateTime)dataInicial).Date).ToList();
            if (dataFinal != null)
                sprints = sprints.Where(sprint => sprint.DataFinal.Date <= ((DateTime)dataFinal).Date).ToList();

            sprintVM.SprintList = sprints.ToPagedList(pageNumber, 10);
            return View(sprintVM);
        }

        public ActionResult Details(int Id)
        {
            var sprintVM = new SprintVM();
            sprintVM.SprintView = _dbContext.Sprints.Include("SprintBacklogs.ProductBacklog")
                                                    .Include("SprintBacklogs.Tarefas")
                                                    .Include("Reunioes")
                                                    .Where(s => s.Id == Id)
                                                    .FirstOrDefault();

            return View(sprintVM);
        }
    }
}