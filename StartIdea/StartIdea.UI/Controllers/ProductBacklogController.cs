using StartIdea.UI.ViewModels;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using StartIdea.Model.Scrum.Artefatos;
using System;

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

            return View(productBacklogVM.BackLogItem.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Detalhes(int id)
        {
            ProductBacklogItem item = new ProductBacklogItem();
            item.Id = id;
            item.UserStory = @"Teste da user asflj akjfhafkjhasfjkasfj asjfaj fajf hakfh ajkfh afkjhafkjahfkjahsfjkashf jakf jkashf jahf jkah fkja fjkahs fjashf kjasfkjasfk aksf akjs f7
                               afasfaaslfkh afsjk asjf aksjf jaskf askjfh akjsf ajskf hakjshfjkhfjkahsf jaksf hajshfjahsfkajf, alhfsldhçsdghaldgkljdfhg kdj gakjhsjhskl sj jks gj gd";
            item.Tamanho = "PP";
            item.Prioridade = 1;
            item.DataInclusao = new DateTime(2015, 10, 9);

            return View(item);
        }
    }
}