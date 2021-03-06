﻿using PagedList;
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
        private StartIdeaDBContext _dbContext;

        public ProductBacklogController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(int? sprintId, string Descricao, StoryPoint? tamanhos, string filtro, int? pagina)
        {
            var productBacklogVM = new ProductBacklogVM();

            if (Descricao != null)
                pagina = 1;
            else
                Descricao = filtro;

            productBacklogVM.UserStory = Descricao;
            productBacklogVM.StoryPoint = tamanhos;
            productBacklogVM.SprintId = sprintId;

            int pageNumber = (pagina ?? 1);
            IEnumerable<ProductBacklog> backlogs = null;

            if (!string.IsNullOrEmpty(Descricao))
            {
                backlogs = _dbContext.ProductBacklogs.Include("SprintBacklogs")
                                                        .Where(productBacklog => productBacklog.UserStory.ToUpper().Contains(Descricao.ToUpper()))
                                                        .ToList()
                                                        .OrderBy(backlog => backlog.Prioridade);
            }
            else
            {
                backlogs = _dbContext.ProductBacklogs.Include("SprintBacklogs")
                                                        .ToList()
                                                        .OrderBy(backlog => backlog.Prioridade);
            }

            if (tamanhos != null)
                backlogs = backlogs.Where(bi => bi.StoryPoint == tamanhos);
            if (sprintId != null)
                backlogs = backlogs.Where(bl => bl.SprintBacklogs.Count > 0 && bl.SprintBacklogs.FirstOrDefault().SprintId == sprintId);

            productBacklogVM.ProductBacklogList = backlogs.ToPagedList(pageNumber, 7);
            return View(productBacklogVM);
        }

        public ActionResult Details(int id, int? sprintId)
        {
            var productBacklogVM = new ProductBacklogVM();
            productBacklogVM.SprintId = sprintId;
            productBacklogVM.ProductBacklogView = _dbContext.ProductBacklogs.Include("ProductOwner.Usuario")
                                                                            .Include("HistoricoEstimativas.MembroTime.Usuario")
                                                                            .Where(x => x.Id == id)
                                                                            .FirstOrDefault();

            return View(productBacklogVM);
        }
    }
}