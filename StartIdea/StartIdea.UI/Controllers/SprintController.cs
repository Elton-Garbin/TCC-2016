using PagedList;
using StartIdea.DataAccess;
using StartIdea.UI.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    [AllowAnonymous]
    public class SprintController : Controller
    {
        private StartIdeaDBContext dbContext;

        public SprintController(StartIdeaDBContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public ActionResult Index(string contextoBusca, string filtro, DateTime? dataInicial, DateTime? dataFinal, int? pagina)
        {
            var sprintBacklogVM = new SprintVM();

            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtro;

            ViewBag.Filtro = contextoBusca;
            ViewBag.DataInicialAtual = dataInicial;
            ViewBag.DataFinalAtual = dataFinal;

            if (!string.IsNullOrEmpty(contextoBusca))
                sprintBacklogVM.Sprints = dbContext.Sprints.Where(sprint => sprint.Objetivo.ToUpper().Contains(contextoBusca.ToUpper())).ToList();
            else
                sprintBacklogVM.Sprints = dbContext.Sprints.ToList();

            if (dataInicial != null)
                sprintBacklogVM.Sprints = sprintBacklogVM.Sprints.Where(sprint => sprint.DataInicial.Date >= ((DateTime)dataInicial).Date).ToList();
            if (dataFinal != null)
                sprintBacklogVM.Sprints = sprintBacklogVM.Sprints.Where(sprint => sprint.DataFinal.Date <= ((DateTime)dataFinal).Date).ToList();

            int pageNumber = (pagina ?? 1);

            return View(sprintBacklogVM.Sprints.ToPagedList(pageNumber, 10));
        }

        public ActionResult Details(int Id)
        {
            var detalheSprintVM = new DetalheSprintVM();

            detalheSprintVM.sprint = dbContext.Sprints.Include("SprintBacklogs.ProductBacklog")
                                                      .Include("SprintBacklogs.Tarefas")
                                                      .Include("Reunioes")
                                                      .Where(s => s.Id == Id).FirstOrDefault();

            return View(detalheSprintVM);
        }
    }
}