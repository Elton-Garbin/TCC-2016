using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ScrumMaster.Models;
using StartIdea.UI.Areas.ScrumMaster.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ScrumMaster.Controllers
{
    public class ReuniaoPlanejamentoController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public ReuniaoPlanejamentoController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            int SprintAtualId = GetSprintId();
            Reuniao reuniao = _dbContext.Reunioes.FirstOrDefault(r => r.TipoReuniao == TipoReuniao.Planejamento
                                                                   && r.SprintId == SprintAtualId) ?? new Reuniao();

            return View(new ReuniaoVM()
            {
                SprintId = SprintAtualId,
                Ata = reuniao.Ata,
                DataFinal = reuniao.DataFinal,
                DataInicial = reuniao.DataInicial,
                Id = reuniao.Id,
                Local = reuniao.Local
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ReuniaoList,PaginaGrid,Id,DataFinal")] ReuniaoVM reuniaoVM)
        {
            if (ModelState.IsValid)
            {
                var reuniao = new Reuniao()
                {
                    TipoReuniao = TipoReuniao.Planejamento,
                    Local = reuniaoVM.Local,
                    Ata = reuniaoVM.Ata,
                    DataInicial = reuniaoVM.DataInicial,
                    DataFinal = reuniaoVM.DataInicial.AddMinutes(15),
                    SprintId = GetSprintId()
                };

                _dbContext.Reunioes.Add(reuniao);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Index", reuniaoVM);
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
            }

            return View("Index", reuniaoVM);
        }

        private int GetSprintId()
        {
            var sprint = _dbContext.Sprints.FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                              && s.TimeId == CurrentUser.TimeId
                                                              && s.DataInicial <= DateTime.Now
                                                              && s.DataFinal >= DateTime.Now) ?? new Sprint();

            return sprint.Id;
        }
    }
}