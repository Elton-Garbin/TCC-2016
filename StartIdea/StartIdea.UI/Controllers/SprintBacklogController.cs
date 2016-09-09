using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    public class SprintBacklogController : Controller
    {
        private StartIdeaDBContext db = new StartIdeaDBContext();

        public ViewResult Index(string contextoBusca, string filtroAtual, DateTime? dataInicial, DateTime? dataFinal, int? pagina)
        {
            var sprintBacklogVM = new SprintBacklogVM();

            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtroAtual;

            ViewBag.ContextoAtual    = contextoBusca;
            ViewBag.DataInicialAtual = dataInicial;
            ViewBag.DataFinalAtual   = dataFinal;

            if (!string.IsNullOrEmpty(contextoBusca))
                sprintBacklogVM.Sprints = db.Sprints.Where(sprint => sprint.Objetivo.ToUpper().Contains(contextoBusca.ToUpper())).ToList();
            else
                sprintBacklogVM.Sprints = db.Sprints.ToList();

            if (dataInicial != null)
                sprintBacklogVM.Sprints = sprintBacklogVM.Sprints.Where(sprint => sprint.DataInicio.Date >= ((DateTime)dataInicial).Date).ToList();

            int pageSize = 5;
            int pageNumber = (pagina ?? 1);

            return View(sprintBacklogVM.Sprints.ToPagedList(pageNumber, pageSize));
        }
    }
}