using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.Areas.ProductOwner.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ProductOwner.Controllers
{
    public class ProductBacklogController : Controller
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public ActionResult Index(string contextoBusca, 
                                  string filtroAtual, 
                                  int? pagina,
                                  int? id)
        {
            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtroAtual;

            ViewBag.Pagina = pagina;
            ViewBag.FiltroAtual = contextoBusca;

            int pageSize = 5;
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserStory,Prioridade")] ProductBacklog productBacklog, string filtroAtual, int? paginaAtual)
        {
            if (ModelState.IsValid)
            {
                productBacklog.ProductOwnerId = 1; // Remover

                var queryVerificacao = from bk in dbContext.ProductBacklogs
                                       where bk.Prioridade == productBacklog.Prioridade &&
                                       !(from sb in dbContext.SprintBacklogs
                                         select sb.ProductBacklogId).Contains(bk.Id)
                                         orderby bk.Prioridade
                                       select bk;

                if (queryVerificacao.ToList().Count > 0)
                {
                    var backlogsUpdate = from bk in dbContext.ProductBacklogs
                                         where bk.Prioridade >= productBacklog.Prioridade &&
                                         !(from sb in dbContext.SprintBacklogs
                                           select sb.ProductBacklogId).Contains(bk.Id)
                                           orderby bk.Prioridade
                                         select bk;

                    for (int i = 0; i < backlogsUpdate.ToList().Count; i++)
                    {
                        var item = backlogsUpdate.ToList()[i];
                        item.Prioridade++;

                        dbContext.Entry(item).State = EntityState.Modified;

                        if (i + 1 <= backlogsUpdate.ToList().Count)
                        {
                            if (backlogsUpdate.ToList()[i + 1].Prioridade != item.Prioridade)
                            {
                                break;
                            }
                        }
                    }
                }

                dbContext.ProductBacklogs.Add(productBacklog);
                dbContext.SaveChanges();

                return RedirectToAction("Index", "ProductBacklog", new
                {
                    filtroAtual = filtroAtual,
                    pagina = paginaAtual
                });
            }

            return RedirectToAction("Index", "ProductBacklog", new
            {
                filtroAtual = filtroAtual,
                pagina = paginaAtual
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserStory,StoryPoint,Prioridade,DataInclusao,ProductOwnerId")] ProductBacklog productBacklog)
        {
            if (ModelState.IsValid)
            {
                var queryVerificacao = from bk in dbContext.ProductBacklogs
                                       where bk.Prioridade == productBacklog.Prioridade &&
                                       bk.Id != productBacklog.Id &&
                                       !(from sb in dbContext.SprintBacklogs
                                         select sb.ProductBacklogId).Contains(bk.Id)
                                       select bk;

                if (queryVerificacao.ToList().Count > 0)
                {
                    var backlogsUpdate = from bk in dbContext.ProductBacklogs
                                         where bk.Prioridade >= productBacklog.Prioridade &&
                                         bk.Id != productBacklog.Id &&
                                         !(from sb in dbContext.SprintBacklogs
                                           select sb.ProductBacklogId).Contains(bk.Id)
                                         select bk;


                    for (int i = 0; i < backlogsUpdate.ToList().Count; i++)
                    {
                        var item = backlogsUpdate.ToList()[i];
                        item.Prioridade++;

                        dbContext.Entry(item).State = EntityState.Modified;

                        if (backlogsUpdate.ToList()[i + 1].Prioridade != item.Prioridade)
                        {
                            break;
                        }
                    }
                }


                dbContext.Entry(productBacklog).State = EntityState.Modified;
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(productBacklog);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBacklog productBacklog = dbContext.ProductBacklogs.Find(id);
            if (productBacklog == null)
                return HttpNotFound();

            foreach (var item in dbContext.HistoricoEstimativas.Where(x => x.ProductBacklogId == productBacklog.Id).ToList())
                dbContext.HistoricoEstimativas.Remove(item);

            dbContext.ProductBacklogs.Remove(productBacklog);
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}