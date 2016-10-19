using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ScrumMaster.Models;
using StartIdea.UI.Areas.ScrumMaster.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ScrumMaster.Controllers
{
    public class DailyScrumController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public DailyScrumController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(int? PaginaGrid)
        {
            var reuniaoVM = new ReuniaoVM()
            {
                PaginaGrid = (PaginaGrid ?? 1)
            };

            reuniaoVM.ReuniaoList = GetGridDataSource(reuniaoVM.PaginaGrid);

            return View(reuniaoVM);
        }

        public ActionResult Create()
        {
            ViewBag.SprintId = new SelectList(_dbContext.Sprints, "Id", "Objetivo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipoReuniao,Local,DataInicial,DataFinal,Observacao,SprintId")] Reuniao reuniao)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Reunioes.Add(reuniao);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.SprintId = new SelectList(_dbContext.Sprints, "Id", "Objetivo", reuniao.SprintId);
            return View(reuniao);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Reuniao reuniao = _dbContext.Reunioes.Find(id);
            if (reuniao == null)
                return HttpNotFound();

            ViewBag.SprintId = new SelectList(_dbContext.Sprints, "Id", "Objetivo", reuniao.SprintId);
            return View(reuniao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipoReuniao,Local,DataInicial,DataFinal,Observacao,SprintId")] Reuniao reuniao)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(reuniao).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.SprintId = new SelectList(_dbContext.Sprints, "Id", "Objetivo", reuniao.SprintId);
            return View(reuniao);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Reuniao reuniao = _dbContext.Reunioes.Find(id);
            if (reuniao == null)
                return HttpNotFound();

            _dbContext.Reunioes.Remove(reuniao);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private IPagedList<Reuniao> GetGridDataSource(int PaginaGrid)
        {
            var query = from r in _dbContext.Reunioes
                        where r.TipoReuniao == TipoReuniao.Diaria
                           && (from s in _dbContext.Sprints
                               where !s.DataCancelamento.HasValue
                                   && s.DataInicial <= DateTime.Now
                                   && s.DataFinal >= DateTime.Now
                               select s.Id).Contains(r.SprintId)
                        select r;

            return query.ToList().ToPagedList(PaginaGrid, 7);
        }
    }
}
