using StartIdea.DataAccess;
using StartIdea.UI.ViewModels;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    [AllowAnonymous]
    public class TarefaController : Controller
    {
        private StartIdeaDBContext _dbContext;

        public TarefaController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(int? productBacklogId,
                                  int sprintId)
        {
            if (productBacklogId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tarefaVM = new TarefaVM();
            tarefaVM.productBacklog = _dbContext.ProductBacklogs.Include("ProductOwner.Usuario")
                                                                .Include("SprintBacklogs")
                                                                .Where(pb => pb.Id == productBacklogId).FirstOrDefault();

            if (tarefaVM.productBacklog == null)
                return HttpNotFound();

            tarefaVM.sprintId = sprintId;
            tarefaVM.productBacklogId = productBacklogId;

            tarefaVM.tarefasBacklog = _dbContext.Tarefas.Where(t => t.SprintBacklog.ProductBacklogId == productBacklogId).ToList();

            return View(tarefaVM);
        }

        public ActionResult Details(int? tarefaId, int productBacklogId, int sprintId)
        {
            if (tarefaId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tarefaDetalhesVM = new TarefaDetalhesVM();

            tarefaDetalhesVM.tarefa = _dbContext.Tarefas.Include("MembroTime.Usuario")
                                                .Include("SprintBacklog.ProductBacklog")
                                                .Include("SprintBacklog.Sprint")
                                                .Where(t => t.Id == tarefaId).FirstOrDefault();

            if (tarefaDetalhesVM.tarefa == null)
                return HttpNotFound();

            tarefaDetalhesVM.productBacklogId = productBacklogId;
            tarefaDetalhesVM.sprintId = sprintId;

            return View(tarefaDetalhesVM);
        }
    }
}