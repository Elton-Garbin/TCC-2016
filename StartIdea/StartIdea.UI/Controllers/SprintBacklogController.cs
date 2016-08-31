using PagedList;
using StartIdea.UI.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    public class SprintBacklogController : Controller
    {
        public ActionResult Index(string contextoBusca, string filtroAtual, DateTime? dataInicial, DateTime? dataFinal, int? pagina)
        {
            var sprintBacklogVM = new SprintBacklogVM();
            sprintBacklogVM.PreencherSprints();

            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtroAtual;

            ViewBag.ContextoAtual = contextoBusca;

            if (!string.IsNullOrEmpty(contextoBusca))
                sprintBacklogVM.Sprints = sprintBacklogVM.Sprints.Where(bkl => bkl.Objetivo.ToUpper().Contains(contextoBusca.ToUpper()));

            int pageSize = 5;
            int pageNumber = (pagina ?? 1);

            return View(sprintBacklogVM.Sprints.ToPagedList(pageNumber, pageSize));
        }
    }
}