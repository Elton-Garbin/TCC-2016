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
            var dailyScrumVM = new DailyScrumVM()
            {
                PaginaGrid = (PaginaGrid ?? 1),
                SprintId = GetSprintAtual().Id
            };

            dailyScrumVM.ReuniaoList = GetGridDataSource(dailyScrumVM.PaginaGrid);

            return View(dailyScrumVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ReuniaoList,PaginaGrid,Id,DataFinal,SprintId")] DailyScrumVM dailyScrumVM)
        {
            if (ModelState.IsValid)
            {
                Sprint SprintAtual = GetSprintAtual();

                if (_dbContext.Reunioes.Any(r => r.SprintId == SprintAtual.Id
                                              && r.TipoReuniao == TipoReuniao.Diaria
                                              && DbFunctions.TruncateTime(r.DataInicial) == dailyScrumVM.DataInicial.Date))
                {
                    ModelState.AddModelError("", "Deve haver apenas uma reunião diária por dia.");
                    return View(dailyScrumVM);
                }
                else if (_dbContext.Reunioes.Count(r => r.SprintId == SprintAtual.Id
                                                     && r.TipoReuniao == TipoReuniao.Diaria) >= 14)
                {
                    ModelState.AddModelError("", "Quantidade de reunião diária não deve ser superior a 14.");
                    return View(dailyScrumVM);
                }
                else if (!(dailyScrumVM.DataInicial >= SprintAtual.DataInicial &&
                           dailyScrumVM.DataInicial <= SprintAtual.DataFinal))
                {
                    ModelState.AddModelError("", "Data da reunião deve estar dentro do intervalo da sprint atual.");
                    return View(dailyScrumVM);
                }

                var reuniao = new Reuniao()
                {
                    TipoReuniao = TipoReuniao.Diaria,
                    Local = dailyScrumVM.Local,
                    Ata = dailyScrumVM.Ata,
                    DataInicial = dailyScrumVM.DataInicial,
                    DataFinal = dailyScrumVM.DataInicial.AddMinutes(15),
                    SprintId = SprintAtual.Id
                };

                _dbContext.Reunioes.Add(reuniao);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(dailyScrumVM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Reuniao reuniao = _dbContext.Reunioes.Find(id);
            if (reuniao == null)
                return HttpNotFound();

            var dailyScrumVM = new DailyScrumVM()
            {
                Id = reuniao.Id,
                Ata = reuniao.Ata,
                DataInicial = reuniao.DataInicial,
                Local = reuniao.Local
            };

            return View(dailyScrumVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "ReuniaoList,PaginaGrid,DataFinal,SprintId")] DailyScrumVM dailyScrumVM)
        {
            if (ModelState.IsValid)
            {
                Reuniao reuniao = _dbContext.Reunioes.Include(r => r.Sprint)
                                                     .Where(r => r.Id == dailyScrumVM.Id)
                                                     .FirstOrDefault();

                if (_dbContext.Reunioes.Any(r => r.SprintId == reuniao.SprintId
                                              && r.Id != reuniao.Id
                                              && r.TipoReuniao == TipoReuniao.Diaria
                                              && DbFunctions.TruncateTime(r.DataInicial) == dailyScrumVM.DataInicial.Date))
                {
                    ModelState.AddModelError("", "Deve haver apenas uma reunião diária por dia.");
                    return View(dailyScrumVM);
                }
                else if (!(dailyScrumVM.DataInicial >= reuniao.Sprint.DataInicial &&
                           dailyScrumVM.DataInicial <= reuniao.Sprint.DataFinal))
                {
                    ModelState.AddModelError("", "Data da reunião deve estar dentro do intervalo da sprint atual.");
                    return View(dailyScrumVM);
                }

                reuniao.Local = dailyScrumVM.Local;
                reuniao.Ata = dailyScrumVM.Ata;
                reuniao.DataInicial = dailyScrumVM.DataInicial;
                reuniao.DataFinal = dailyScrumVM.DataInicial.AddMinutes(15);

                _dbContext.Entry(reuniao).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(dailyScrumVM);
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
                        orderby r.DataInicial
                        select r;

            return query.ToList().ToPagedList(PaginaGrid, 10);
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
