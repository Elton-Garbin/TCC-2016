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
            tarefaVM.ProductBacklogView = _dbContext.ProductBacklogs.Include("ProductOwner.Usuario")
                                                                    .Include("SprintBacklogs")
                                                                    .Where(pb => pb.Id == productBacklogId)
                                                                    .FirstOrDefault();

            if (tarefaVM.ProductBacklogView == null)
                return HttpNotFound();

            tarefaVM.SprintId = sprintId;
            tarefaVM.ProductBacklogId = productBacklogId;

            tarefaVM.TarefaList = _dbContext.Tarefas.Where(t => t.SprintBacklog.ProductBacklogId == productBacklogId).ToList();
            return View(tarefaVM);
        }

        public ActionResult Details(int? tarefaId, int productBacklogId, int sprintId)
        {
            if (tarefaId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tarefaVM = new TarefaVM();
            tarefaVM.TarefaView = _dbContext.Tarefas.Include("MembroTime.Usuario")
                                                    .Include("SprintBacklog.ProductBacklog")
                                                    .Include("SprintBacklog.Sprint")
                                                    .Where(t => t.Id == tarefaId)
                                                    .FirstOrDefault();

            if (tarefaVM.TarefaView == null)
                return HttpNotFound();

            tarefaVM.ProductBacklogId = productBacklogId;
            tarefaVM.SprintId = sprintId;

            return View(tarefaVM);
        }
    }
}