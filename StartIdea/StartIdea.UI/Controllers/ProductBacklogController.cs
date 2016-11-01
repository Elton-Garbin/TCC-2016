using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    [AllowAnonymous]
    public class ProductBacklogController : Controller
    {
        private StartIdeaDBContext dbContext;

        public ProductBacklogController(StartIdeaDBContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public ActionResult Index(int? sprintId, string Descricao, StoryPoint? tamanhos, string filtro, int? pagina)
        {
            var productBacklogVM = new ProductBacklogVM();

            if (Descricao != null)
                pagina = 1;
            else
                Descricao = filtro;

            productBacklogVM.Descricao = Descricao;
            productBacklogVM.tamanhos = tamanhos;
            productBacklogVM.sprintId = sprintId;

            int pageSize = 5;
            int pageNumber = (pagina ?? 1);
            IEnumerable<ProductBacklog> backlogItem = null;

            if (!string.IsNullOrEmpty(Descricao))
            {
                backlogItem = dbContext.ProductBacklogs.Include("SprintBacklogs")
                                       .Where(productBacklog => productBacklog.UserStory.ToUpper().Contains(Descricao.ToUpper()))
                                       .ToList()
                                       .OrderBy(backlog => backlog.Prioridade);
            }
            else
            {
                backlogItem = dbContext.ProductBacklogs.Include("SprintBacklogs")
                                       .ToList()
                                       .OrderBy(backlog => backlog.Prioridade);
            }

            if (tamanhos != null)
            {
                backlogItem = backlogItem.Where(bi => bi.StoryPoint == tamanhos);
            }

            if(sprintId != null)
            {
                backlogItem = backlogItem.Where(bl => bl.SprintBacklogs.Count > 0 && bl.SprintBacklogs.FirstOrDefault().SprintId == sprintId);
            }

            productBacklogVM.BackLogItem = backlogItem.ToPagedList(pageNumber, pageSize);
            return View(productBacklogVM);
        }

        public ActionResult Detalhes(int id, int? sprintId)
        {
            var detalheProductBacklogVM = new DetalheProductBacklogVM();

            detalheProductBacklogVM.productBacklog = dbContext.ProductBacklogs
                                                              .Include("ProductOwner.Usuario")
                                                              .Include("HistoricoEstimativas.MembroTime.Usuario")
                                                              .Where(x => x.Id == id)
                                                              .FirstOrDefault();

            detalheProductBacklogVM.sprintId = sprintId;

            return View(detalheProductBacklogVM);
        }
    }
}