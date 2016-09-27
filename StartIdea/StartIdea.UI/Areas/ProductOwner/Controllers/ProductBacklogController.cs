using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.Areas.ProductOwner.ViewModels;
using System;
using System.Collections.Generic;
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
                                  string filtro, 
                                  DateTime? filtroDataInicial,
                                  DateTime? filtroDataFinal,
                                  int? pagina,
                                  int? id)
        {
            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtro;

            ViewBag.Pagina = pagina;
            ViewBag.Filtro = contextoBusca;

            var query = from pb in dbContext.ProductBacklogs
                        where !(from sb in dbContext.SprintBacklogs
                                select sb.ProductBacklogId)
                                .Contains(pb.Id)
                        orderby pb.Prioridade
                        select pb;

            var productBacklogVM = new ProductBacklogVM();
            List<ProductBacklog> listBacklogs = null;
            if (!string.IsNullOrEmpty(contextoBusca))
            {
                listBacklogs = query.Where(productBacklog => productBacklog.UserStory
                                                                           .ToUpper()
                                                                           .Contains(contextoBusca.ToUpper()))
                                                                           .ToList();
            }
            else
                listBacklogs = query.ToList();

            if (filtroDataInicial != null)
            {
                listBacklogs = listBacklogs.Where(x => x.DataInclusao.Date >= ((DateTime)filtroDataInicial).Date).ToList();
            }

            if (filtroDataFinal != null)
            {
                listBacklogs = listBacklogs.Where(x => x.DataInclusao.Date <= ((DateTime)filtroDataFinal).Date).ToList();
            }

            productBacklogVM.filtroDataInicial = filtroDataInicial;
            productBacklogVM.filtroDataFinal = filtroDataFinal;

            productBacklogVM.ProductBacklogCreateVM = new ProductBacklogCreateVM();
            productBacklogVM.ProductBacklogCreateVM.filtroDataInicial = filtroDataInicial;
            productBacklogVM.ProductBacklogCreateVM.filtroDataFinal = filtroDataFinal;

            ViewBag.FiltroDataInicial = filtroDataInicial;
            ViewBag.FiltroDataFinal = filtroDataFinal;

            if (id != null)
            {
                ProductBacklog productBacklog = dbContext.ProductBacklogs.Find(id);
                if (productBacklog == null)
                    return HttpNotFound();

                productBacklogVM.ProductBacklogEdit = productBacklog;
                productBacklogVM.DisplayEdit = "Show";
            }

            int pageSize = 7;
            int pageNumber = (pagina ?? 1);

            productBacklogVM.ProductBacklogList = listBacklogs.ToPagedList(pageNumber, pageSize);

            return View(productBacklogVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserStory,Prioridade")] ProductBacklog productBacklog, string filtro, int? pagina, DateTime? filtroDataInicial, DateTime? filtroDataFinal)
        {
            if (ModelState.IsValid)
            {
                productBacklog.ProductOwnerId = 1; // Remover
                
                ReordenarPrioridades(0, productBacklog.Prioridade);
                
                dbContext.ProductBacklogs.Add(productBacklog);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index", "ProductBacklog", new
            {
                filtro            = filtro,
                pagina            = pagina,
                filtroDataInicial = filtroDataInicial,
                filtroDataFinal   = filtroDataFinal
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserStory,StoryPoint,Prioridade,DataInclusao")] ProductBacklog productBacklog, string filtro, int? pagina, short prioridadeAtual, DateTime? filtroDataInicial, DateTime? filtroDataFinal)
        {
            if (ModelState.IsValid)
            {
                productBacklog.ProductOwnerId = 1; // Remover

                if (prioridadeAtual != productBacklog.Prioridade)
                    ReordenarPrioridades(productBacklog.Id, productBacklog.Prioridade);

                dbContext.Entry(productBacklog).State = EntityState.Modified;
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index", "ProductBacklog", new
            {
                filtro = filtro,
                pagina = pagina,
                filtroDataInicial = filtroDataInicial,
                filtroDataFinal = filtroDataFinal
            });
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

        private void ReordenarPrioridades(int ProductBacklogId, short Prioridade)
        {
            var query = from pb in dbContext.ProductBacklogs
                        where pb.Id != ProductBacklogId
                           && pb.Prioridade >= Prioridade
                           && !(from sb in dbContext.SprintBacklogs
                                select sb.ProductBacklogId)
                                .Contains(pb.Id)
                        orderby pb.Prioridade
                        select pb;

            for (int i = 0; i < query.ToList().Count; i++)
            {
                var item = query.ToArray()[i];

                if (item.Prioridade != Prioridade)
                {
                    var prioridadeAnterior = Prioridade;

                    if (i > 0)
                        prioridadeAnterior = query.ToArray()[i - 1].Prioridade;

                    if (prioridadeAnterior < item.Prioridade)
                        break;
                }

                item.Prioridade++;
                dbContext.Entry(item).State = EntityState.Modified;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}