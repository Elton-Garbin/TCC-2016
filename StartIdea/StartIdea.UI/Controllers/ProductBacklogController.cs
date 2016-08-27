using StartIdea.UI.ViewModels;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace StartIdea.UI.Controllers
{
    public class ProductBacklogController : Controller
    {
        public ActionResult Index(string contextoBusca, string filtroAtual, int? pagina)
        {
            var productBacklogVM = new ProductBacklogVM();
            productBacklogVM.PreencheBackLogItem();

            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtroAtual;

            ViewBag.FiltroAtual = contextoBusca;

            if (!string.IsNullOrEmpty(contextoBusca))
            {
                productBacklogVM.BackLogItem = productBacklogVM.BackLogItem.Where(bkl => bkl.UserStory.ToUpper().Contains(contextoBusca.ToUpper()));
            }

            int pageSize = 5;
            int pageNumber = (pagina ?? 1);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("ProductBacklogGrid", productBacklogVM.BackLogItem.ToPagedList(pageNumber, pageSize))
                : View(productBacklogVM.BackLogItem.ToPagedList(pageNumber, pageSize));
        }
    }
}