using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.Areas.TeamMember.Models;
using StartIdea.UI.Areas.TeamMember.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.TeamMember.Controllers
{
    public class ProductBacklogController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public ProductBacklogController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

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

            var query = from pb in _dbContext.ProductBacklogs
                        where !(from sb in _dbContext.SprintBacklogs
                                select sb.ProductBacklogId)
                                .Contains(pb.Id) &&
                        pb.Prioridade > 0
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
                ProductBacklog productBacklog = _dbContext.ProductBacklogs
                                                         .Include("ProductOwner.Usuario")
                                                         .SingleOrDefault(x => x.Id == id);
                if (productBacklog == null)
                    return HttpNotFound();


                productBacklogVM.Id = productBacklog.Id;
                productBacklogVM.UserStory = productBacklog.UserStory;
                productBacklogVM.Prioridade = productBacklog.Prioridade;
                productBacklogVM.ProductOwner = productBacklog.ProductOwner;
                productBacklogVM.DataInclusao = productBacklog.DataInclusao;
                productBacklogVM.StoryPoint = productBacklog.StoryPoint;
                productBacklogVM.DisplayEdit = "Show";
            }

            return View(productBacklogVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StoryPoint")] ProductBacklogVM productBacklog, string filtro, int? pagina)
        {
            var productBacklogAux = _dbContext.ProductBacklogs.Find(productBacklog.Id);
            productBacklogAux.StoryPoint = productBacklog.StoryPoint;

            productBacklogAux.HistoricoEstimativas.Add(new HistoricoEstimativa()
            {
                DataInclusao = DateTime.Now,
                ProductBacklogId = productBacklog.Id,
                StoryPoint = productBacklog.StoryPoint,
                MembroTimeId = CurrentUser.PerfilId
            });

            _dbContext.Entry(productBacklogAux).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "ProductBacklog", new
            {
                filtro = filtro,
                pagina = pagina
            });
        }
    }
}