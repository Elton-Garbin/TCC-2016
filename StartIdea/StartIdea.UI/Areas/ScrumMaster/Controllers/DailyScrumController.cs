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
        public ActionResult Create([Bind(Exclude = "ReuniaoList,PaginaGrid,Id,DataFinal,SprintId")] ReuniaoVM reuniaoVM)
        {
            if (ModelState.IsValid)
            {
                int SprintAtualId = GetSprintAtual().Id;

                var reuniao = new Reuniao()
                {
                    TipoReuniao = TipoReuniao.Diaria,
                    Local = reuniaoVM.Local,
                    Ata = reuniaoVM.Ata,
                    DataInicial = reuniaoVM.DataInicial,
                    DataFinal = reuniaoVM.DataInicial.AddMinutes(15),
                    SprintId = SprintAtualId
                };

                _dbContext.Reunioes.Add(reuniao);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(reuniaoVM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Reuniao reuniao = _dbContext.Reunioes.Find(id);
            if (reuniao == null)
                return HttpNotFound();

            var reuniaoVM = new ReuniaoVM()
            {
                Id = reuniao.Id,
                Ata = reuniao.Ata,
                DataFinal = reuniao.DataFinal,
                DataInicial = reuniao.DataInicial,
                Local = reuniao.Local
            };

            return View(reuniaoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "ReuniaoList,PaginaGrid,DataFinal,SprintId")] ReuniaoVM reuniaoVM)
        {
            if (ModelState.IsValid)
            {
                Reuniao reuniao = _dbContext.Reunioes.Find(reuniaoVM.Id);
                reuniao.Local = reuniaoVM.Local;
                reuniao.Ata = reuniaoVM.Ata;
                reuniao.DataInicial = reuniaoVM.DataInicial;
                reuniao.DataFinal = reuniaoVM.DataInicial.AddMinutes(15);

                _dbContext.Entry(reuniao).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(reuniaoVM);
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
            int SprintAtualId = GetSprintAtual().Id;

            var query = from r in _dbContext.Reunioes
                        where r.TipoReuniao == TipoReuniao.Diaria
                           && r.SprintId == SprintAtualId
                        select r;

            return query.ToList().ToPagedList(PaginaGrid, 7);
        }

        private Sprint GetSprintAtual()
        {
            return _dbContext.Sprints.FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                        && s.TimeId == CurrentUser.TimeId
                                                        && s.DataInicial <= DateTime.Now
                                                        && s.DataFinal >= DateTime.Now) ?? new Sprint();
        }
    }
}
