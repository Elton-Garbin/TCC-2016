using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.Areas.ProductOwner.Models;
using StartIdea.UI.Areas.ProductOwner.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ProductOwner.Controllers
{
    public class ProductBacklogController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public ProductBacklogController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(string FiltroUserStory,
                                  DateTime? FiltroDataInicial,
                                  DateTime? FiltroDataFinal,
                                  int? PaginaGrid)
        {
            var productBacklogVM = new ProductBacklogVM();
            productBacklogVM.PaginaGrid = (PaginaGrid ?? 1);
            productBacklogVM.FiltroUserStory = FiltroUserStory;
            productBacklogVM.FiltroDataInicial = Convert.ToString(FiltroDataInicial);
            productBacklogVM.FiltroDataFinal = Convert.ToString(FiltroDataFinal);

            productBacklogVM.ProductBacklogList = GetGridPagedDataSource(productBacklogVM);

            return View(productBacklogVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductBacklogVM productBacklogVM)
        {
            if (ModelState.IsValid)
            {
                ReordenarPrioridades(0, productBacklogVM.Prioridade);

                var productBacklog = new ProductBacklog()
                {
                    UserStory = productBacklogVM.UserStory,
                    Prioridade = productBacklogVM.Prioridade,
                    ProductOwnerId = CurrentUser.PerfilId
                };

                _dbContext.ProductBacklogs.Add(productBacklog);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(productBacklogVM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBacklog productBacklog = _dbContext.ProductBacklogs.Find(id);
            if (productBacklog == null)
                return HttpNotFound();

            var productBacklogVM = new ProductBacklogVM()
            {
                Id = productBacklog.Id,
                UserStory = productBacklog.UserStory,
                Prioridade = productBacklog.Prioridade
            };

            return View(productBacklogVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductBacklogVM productBacklogVM, short prioridadeAtual)
        {
            if (ModelState.IsValid)
            {
                if (prioridadeAtual != productBacklogVM.Prioridade)
                    ReordenarPrioridades(productBacklogVM.Id, productBacklogVM.Prioridade);

                ProductBacklog productBacklog = _dbContext.ProductBacklogs.Find(productBacklogVM.Id);
                productBacklog.ProductOwnerId = CurrentUser.PerfilId;
                productBacklog.UserStory = productBacklogVM.UserStory;
                productBacklog.Prioridade = productBacklogVM.Prioridade;

                _dbContext.Entry(productBacklog).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(productBacklogVM);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBacklog productBacklog = _dbContext.ProductBacklogs.Find(id);
            if (productBacklog == null)
                return HttpNotFound();

            foreach (var item in _dbContext.HistoricoEstimativas.Where(x => x.ProductBacklogId == productBacklog.Id).ToList())
                _dbContext.HistoricoEstimativas.Remove(item);

            _dbContext.ProductBacklogs.Remove(productBacklog);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Report(string FiltroUserStory,
                                   DateTime? FiltroDataInicial,
                                   DateTime? FiltroDataFinal)
        {
            var productBacklogVM = new ProductBacklogVM();
            productBacklogVM.FiltroUserStory = FiltroUserStory;
            productBacklogVM.FiltroDataInicial = Convert.ToString(FiltroDataInicial);
            productBacklogVM.FiltroDataFinal = Convert.ToString(FiltroDataFinal);

            productBacklogVM.ProductBacklogReport = GetGridDataSource(productBacklogVM);

            return View(productBacklogVM);
        }

        private IPagedList<ProductBacklog> GetGridPagedDataSource(ProductBacklogVM productBacklogVM)
        {
            return GetGridDataSource(productBacklogVM).ToList().ToPagedList(Convert.ToInt32(productBacklogVM.PaginaGrid), 7);
        }

        private IEnumerable<ProductBacklog> GetGridDataSource(ProductBacklogVM productBacklogVM)
        {
            IEnumerable<ProductBacklog> listBacklog = from pb in _dbContext.ProductBacklogs
                                                      where !(from sb in _dbContext.SprintBacklogs
                                                              select sb.ProductBacklogId)
                                                              .Contains(pb.Id)
                                                      orderby pb.Prioridade
                                                      select pb;

            if (!string.IsNullOrEmpty(productBacklogVM.FiltroUserStory))
            {
                listBacklog = listBacklog.Where(productBacklog => productBacklog.UserStory
                                                                                  .ToUpper()
                                                                                  .Contains(productBacklogVM.FiltroUserStory.ToUpper()));
            }
            if (!string.IsNullOrEmpty(productBacklogVM.FiltroDataInicial))
                listBacklog = listBacklog.Where(x => x.DataInclusao.Date >= Convert.ToDateTime(productBacklogVM.FiltroDataInicial).Date);
            if (!string.IsNullOrEmpty(productBacklogVM.FiltroDataFinal))
                listBacklog = listBacklog.Where(x => x.DataInclusao.Date <= Convert.ToDateTime(productBacklogVM.FiltroDataFinal).Date);

            return listBacklog;
        }

        private void ReordenarPrioridades(int ProductBacklogId, short Prioridade)
        {
            var query = from pb in _dbContext.ProductBacklogs
                        where pb.Id != ProductBacklogId
                           && pb.Prioridade >= Prioridade
                           && !(from sb in _dbContext.SprintBacklogs
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
                _dbContext.Entry(item).State = EntityState.Modified;
            }
        }
    }
}