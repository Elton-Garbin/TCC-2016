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

        public ActionResult Index(string FiltroUserStory,
                                  DateTime? FiltroDataInicial,
                                  DateTime? FiltroDataFinal,
                                  int? PaginaGrid,
                                  int? IdEdit,
                                  string DisplayCreate)
        {
            var productBacklogVM = new ProductBacklogVM();
            productBacklogVM.PaginaGrid = (PaginaGrid ?? 1);
            productBacklogVM.FiltroUserStory = FiltroUserStory;
            productBacklogVM.FiltroDataInicial = Convert.ToString(FiltroDataInicial);
            productBacklogVM.FiltroDataFinal = Convert.ToString(FiltroDataFinal);
            productBacklogVM.DisplayCreate = DisplayCreate;

            if ((IdEdit ?? 0) > 0)
            {
                ProductBacklog productBacklog = dbContext.ProductBacklogs.Find(IdEdit);
                if (productBacklog == null)
                    return HttpNotFound();

                productBacklogVM.Id = productBacklog.Id;
                productBacklogVM.UserStory = productBacklog.UserStory;
                productBacklogVM.Prioridade = productBacklog.Prioridade;
                productBacklogVM.DisplayEdit = "show";
            }

            productBacklogVM.ProductBacklogList = GetGridDataSource(productBacklogVM); 

            return View(productBacklogVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductBacklogVM productBacklogVM)
        {
            if (ModelState.IsValid)
            {
                ReordenarPrioridades(0, productBacklogVM.Prioridade);

                ProductBacklog productBacklog = new ProductBacklog()
                {
                    UserStory = productBacklogVM.UserStory,
                    Prioridade = productBacklogVM.Prioridade,
                    ProductOwnerId = 1 // Remover
                };

                dbContext.ProductBacklogs.Add(productBacklog);
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Index", productBacklogVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductBacklogVM productBacklogVM, short prioridadeAtual)
        {
            if (ModelState.IsValid)
            {
                if (prioridadeAtual != productBacklogVM.Prioridade)
                    ReordenarPrioridades(productBacklogVM.Id, productBacklogVM.Prioridade);

                ProductBacklog productBacklog = dbContext.ProductBacklogs.Find(productBacklogVM.Id);
                productBacklog.ProductOwnerId = 1; // Remover
                productBacklog.UserStory = productBacklogVM.UserStory;
                productBacklog.Prioridade = productBacklogVM.Prioridade;

                dbContext.Entry(productBacklog).State = EntityState.Modified;
                dbContext.SaveChanges();

                productBacklogVM.DisplayEdit = string.Empty;
            }

            productBacklogVM.ProductBacklogList = GetGridDataSource(productBacklogVM);

            return View("Index", productBacklogVM);
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

        private IPagedList<ProductBacklog> GetGridDataSource(ProductBacklogVM productBacklogVM)
        {
            IEnumerable<ProductBacklog> listBacklogs = from pb in dbContext.ProductBacklogs
                                                       where !(from sb in dbContext.SprintBacklogs
                                                               select sb.ProductBacklogId)
                                                               .Contains(pb.Id)
                                                       orderby pb.Prioridade
                                                       select pb;

            if (!string.IsNullOrEmpty(productBacklogVM.FiltroUserStory))
            {
                listBacklogs = listBacklogs.Where(productBacklog => productBacklog.UserStory
                                                                                  .ToUpper()
                                                                                  .Contains(productBacklogVM.FiltroUserStory.ToUpper()));
            }
            if (!string.IsNullOrEmpty(productBacklogVM.FiltroDataInicial))
                listBacklogs = listBacklogs.Where(x => x.DataInclusao.Date >= Convert.ToDateTime(productBacklogVM.FiltroDataInicial).Date);
            if (!string.IsNullOrEmpty(productBacklogVM.FiltroDataFinal))
                listBacklogs = listBacklogs.Where(x => x.DataInclusao.Date <= Convert.ToDateTime(productBacklogVM.FiltroDataFinal).Date);

            return listBacklogs.ToList().ToPagedList(Convert.ToInt32(productBacklogVM.PaginaGrid), 7);
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