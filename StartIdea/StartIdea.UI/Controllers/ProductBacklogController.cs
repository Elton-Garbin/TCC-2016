using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    public class ProductBacklogController : Controller
    {
        private StartIdeaDBContext dbContext;

        public ProductBacklogController(StartIdeaDBContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public ActionResult Index(string contextoBusca, string filtro, int? pagina)
        {
            var productBacklogVM = new ProductBacklogVM();

            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtro;

            ViewBag.Filtro = contextoBusca;

            if (!string.IsNullOrEmpty(contextoBusca))
            {
                productBacklogVM.BackLogItem = dbContext.ProductBacklogs
                                                        .Where(productBacklog => productBacklog.UserStory.ToUpper().Contains(contextoBusca.ToUpper()))
                                                        .ToList()
                                                        .OrderBy(backlog => backlog.Prioridade);
            }
            else
            {
                productBacklogVM.BackLogItem = dbContext.ProductBacklogs
                                                        .ToList()
                                                        .OrderBy(backlog => backlog.Prioridade);
            }

            int pageSize = 5;
            int pageNumber = (pagina ?? 1);

            return View(productBacklogVM.BackLogItem.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Detalhes(int id)
        {
            //ProductBacklog backlog = dbContext.ProductBacklogs.Find(id);

            ProductBacklog backlog = dbContext.ProductBacklogs
                                              .Include("ProductOwner.Usuario")
                                              .Include("HistoricoEstimativas.MembroTime.Usuario")
                                              .Where(x => x.Id == id)
                                              .FirstOrDefault();


            return View(backlog);
        }
    }
}