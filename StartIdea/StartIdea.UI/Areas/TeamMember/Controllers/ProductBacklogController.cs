using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.Areas.TeamMember.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.TeamMember.Controllers
{
    public class ProductBacklogController : Controller
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public ActionResult Index(string contextoBusca, 
                                  string filtro, 
                                  int? pagina,
                                  int? id)
        {
            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtro;

            ViewBag.Pagina = pagina;
            ViewBag.Filtro = contextoBusca;

            int pageSize = 7;
            int pageNumber = (pagina ?? 1);

            var query = from pb in dbContext.ProductBacklogs
                        where !(from sb in dbContext.SprintBacklogs
                                select sb.ProductBacklogId)
                                .Contains(pb.Id)
                        orderby pb.Prioridade
                        select pb;

            var productBacklogVM = new ProductBacklogVM();
            if (!string.IsNullOrEmpty(contextoBusca))
            {
                productBacklogVM.ProductBacklogList = query.Where(productBacklog => productBacklog.UserStory.ToUpper().Contains(contextoBusca.ToUpper()))
                                                           .ToList()
                                                           .ToPagedList(pageNumber, pageSize);
            }
            else
            {
                productBacklogVM.ProductBacklogList = query.ToList()
                                                           .ToPagedList(pageNumber, pageSize);
            }

            if (id != null)
            {
                ProductBacklog productBacklog = dbContext.ProductBacklogs.Find(id);
                if (productBacklog == null)
                    return HttpNotFound();

                productBacklogVM.ProductBacklogEdit = productBacklog;
                productBacklogVM.DisplayEdit = "Show";
            }

            return View(productBacklogVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StoryPoint")] ProductBacklog productBacklog, string filtro, int? pagina)
        {
            var productBacklogAux = dbContext.ProductBacklogs.Find(productBacklog.Id);
            productBacklogAux.StoryPoint = productBacklog.StoryPoint;

            dbContext.Entry(productBacklogAux).State = EntityState.Modified;
            dbContext.SaveChanges();

            return RedirectToAction("Index", "ProductBacklog", new
            {
                filtro = filtro,
                pagina = pagina
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}