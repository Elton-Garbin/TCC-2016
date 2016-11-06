using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Areas.ScrumMaster.Models;
using StartIdea.UI.Areas.ScrumMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ScrumMaster.Controllers
{
    public class SprintController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public SprintController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            var sprint = _dbContext.Sprints.Where(s => !s.DataCancelamento.HasValue
                                               && s.TimeId == CurrentUser.TimeId
                                               && s.DataInicial > DateTime.Now)
                                           .OrderByDescending(s => s.DataInicial)
                                           .FirstOrDefault() ?? new Sprint();

            var sprintVM = new SprintVM()
            {
                SprintAtual = GetSprintAtual(),
                Id = sprint.Id,
                DataInicial = sprint.DataInicial,
                DataFinal = sprint.DataFinal,
                Objetivo = sprint.Objetivo
            };

            #region Reunião Planejamento
            var reuniaoPlanejamento = _dbContext.Reunioes.Where(r => r.SprintId == sprintVM.Id
                                                                  && r.TipoReuniao == TipoReuniao.Planejamento)
                                                .FirstOrDefault() ?? new Reuniao();

            sprintVM.IdRP = reuniaoPlanejamento.Id;
            sprintVM.LocalRP = reuniaoPlanejamento.Local;
            sprintVM.DataInicialRP = reuniaoPlanejamento.DataInicial;
            sprintVM.DataFinalRP = reuniaoPlanejamento.DataFinal;
            #endregion

            #region Reunião Diária
            var reuniaoDiaria = _dbContext.Reunioes.Where(r => r.SprintId == sprintVM.Id
                                                            && r.TipoReuniao == TipoReuniao.Diaria)
                                                   .FirstOrDefault() ?? new Reuniao();

            sprintVM.LocalRD = reuniaoDiaria.Local;
            #endregion

            #region Reunião Revisão
            var reuniaoRevisao = _dbContext.Reunioes.Where(r => r.SprintId == sprintVM.Id
                                                             && r.TipoReuniao == TipoReuniao.Revisao)
                                                    .FirstOrDefault() ?? new Reuniao();

            sprintVM.IdRV = reuniaoRevisao.Id;
            sprintVM.LocalRV = reuniaoRevisao.Local;
            sprintVM.DataInicialRV = reuniaoRevisao.DataInicial;
            sprintVM.DataFinalRV = reuniaoRevisao.DataFinal;
            #endregion

            #region Reunião Retrospectiva
            var reuniaoRetrospectiva = _dbContext.Reunioes.Where(r => r.SprintId == sprintVM.Id
                                                                   && r.TipoReuniao == TipoReuniao.Retrospectiva)
                                                          .FirstOrDefault() ?? new Reuniao();

            sprintVM.IdRT = reuniaoRetrospectiva.Id;
            sprintVM.LocalRT = reuniaoRetrospectiva.Local;
            sprintVM.DataInicialRT = reuniaoRetrospectiva.DataInicial;
            sprintVM.DataFinalRT = reuniaoRetrospectiva.DataFinal;
            #endregion

            return View(sprintVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SprintVM sprintVM)
        {
            sprintVM.SprintAtual = GetSprintAtual();

            if (ModelState.IsValid)
            {
                if (sprintVM.SprintAtual.Id > 0 && sprintVM.DataInicial <= sprintVM.SprintAtual.DataFinal)
                {
                    ModelState.AddModelError("DataInicial", "[Sprint] Data Inicial deve ser maior do que a data final da sprint atual.");
                    return View("Index", sprintVM);
                }

                var sprint = new Sprint()
                {
                    Objetivo = sprintVM.Objetivo,
                    DataInicial = sprintVM.DataInicial,
                    DataFinal = sprintVM.DataFinal,
                    TimeId = CurrentUser.TimeId
                };

                _dbContext.Sprints.Add(sprint);
                _dbContext.SaveChanges();

                var SprintId = sprint.Id;

                #region Reunião Planejamento
                var reuniaoPlanejamento = new Reuniao()
                {
                    TipoReuniao = TipoReuniao.Planejamento,
                    Local = sprintVM.LocalRP,
                    Ata = "...",
                    DataInicial = sprintVM.DataInicialRP,
                    DataFinal = sprintVM.DataFinalRP,
                    SprintId = SprintId
                };

                _dbContext.Reunioes.Add(reuniaoPlanejamento);
                _dbContext.SaveChanges();
                #endregion

                #region Reunião Diária
                GerarDailyScrum(sprintVM);
                #endregion

                #region Reunião Revisão
                var reuniaoRevisao = new Reuniao()
                {
                    TipoReuniao = TipoReuniao.Revisao,
                    Local = sprintVM.LocalRV,
                    Ata = "...",
                    DataInicial = sprintVM.DataInicialRV,
                    DataFinal = sprintVM.DataFinalRV,
                    SprintId = SprintId
                };

                _dbContext.Reunioes.Add(reuniaoRevisao);
                _dbContext.SaveChanges();
                #endregion

                #region Reunião Retrospectiva
                var reuniaoRetrospectiva = new Reuniao()
                {
                    TipoReuniao = TipoReuniao.Retrospectiva,
                    Local = sprintVM.LocalRT,
                    Ata = "...",
                    DataInicial = sprintVM.DataInicialRT,
                    DataFinal = sprintVM.DataFinalRT,
                    SprintId = SprintId
                };

                _dbContext.Reunioes.Add(reuniaoRetrospectiva);
                _dbContext.SaveChanges();
                #endregion

                return RedirectToAction("Index");
            }

            return View("Index", sprintVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SprintVM sprintVM)
        {
            sprintVM.SprintAtual = GetSprintAtual();

            if (ModelState.IsValid)
            {
                if (sprintVM.SprintAtual.Id > 0 && sprintVM.DataInicial <= sprintVM.SprintAtual.DataFinal)
                {
                    ModelState.AddModelError("DataInicial", "[Sprint] Data Inicial deve ser maior do que a data final da sprint atual.");
                    return View("Index", sprintVM);
                }

                var sprint = _dbContext.Sprints.Find(sprintVM.Id);
                sprint.Objetivo = sprintVM.Objetivo;
                sprint.DataInicial = sprintVM.DataInicial;
                sprint.DataFinal = sprintVM.DataFinal;

                _dbContext.Entry(sprint).State = EntityState.Modified;
                _dbContext.SaveChanges();

                #region Reunião Planejamento
                var reuniaoPlanejamento = _dbContext.Reunioes.Find(sprintVM.IdRP);
                reuniaoPlanejamento.Local = sprintVM.LocalRP;
                reuniaoPlanejamento.DataInicial = sprintVM.DataInicialRP;
                reuniaoPlanejamento.DataFinal = sprintVM.DataFinalRP;

                _dbContext.Entry(reuniaoPlanejamento).State = EntityState.Modified;
                _dbContext.SaveChanges();
                #endregion

                #region Reunião Diária
                _dbContext.Reunioes.RemoveRange(_dbContext.Reunioes.Where(r => r.SprintId == sprintVM.Id
                                                                            && r.TipoReuniao == TipoReuniao.Diaria));
                _dbContext.SaveChanges();

                GerarDailyScrum(sprintVM);
                #endregion

                #region Reunião Revisão
                var reuniaoRevisao = _dbContext.Reunioes.Find(sprintVM.IdRV);
                reuniaoRevisao.Local = sprintVM.LocalRV;
                reuniaoRevisao.DataInicial = sprintVM.DataInicialRV;
                reuniaoRevisao.DataFinal = sprintVM.DataFinalRV;

                _dbContext.Entry(reuniaoRevisao).State = EntityState.Modified;
                _dbContext.SaveChanges();
                #endregion

                #region Reunião Retrospectiva
                var reuniaoRetrospectiva = _dbContext.Reunioes.Find(sprintVM.IdRT);
                reuniaoRetrospectiva.Local = sprintVM.LocalRT;
                reuniaoRetrospectiva.DataInicial = sprintVM.DataInicialRT;
                reuniaoRetrospectiva.DataFinal = sprintVM.DataFinalRT;

                _dbContext.Entry(reuniaoRetrospectiva).State = EntityState.Modified;
                _dbContext.SaveChanges();
                #endregion

                return RedirectToAction("Index");
            }
            
            return View("Index", sprintVM);
        }

        private void GerarDailyScrum(SprintVM sprintVM)
        {
            List<Reuniao> dailyScrum = new List<Reuniao>();
            int qtDias = (sprintVM.DataFinalRD - sprintVM.DataInicialRD).Days;

            for (int i = 0; i < qtDias; i++)
            {
                var time = sprintVM.HorarioInicialRD.Split(':');
                var dataBase = sprintVM.DataInicialRD.Date.AddDays(i)
                                                          .AddHours(Convert.ToInt16(time[0]))
                                                          .AddMinutes(Convert.ToInt16(time[1]));

                if (dataBase.DayOfWeek == DayOfWeek.Sunday && !sprintVM.WorkSun)
                    continue;
                if (dataBase.DayOfWeek == DayOfWeek.Monday && !sprintVM.WorkMon)
                    continue;
                if (dataBase.DayOfWeek == DayOfWeek.Tuesday && !sprintVM.WorkTue)
                    continue;
                if (dataBase.DayOfWeek == DayOfWeek.Wednesday && !sprintVM.WorkWed)
                    continue;
                if (dataBase.DayOfWeek == DayOfWeek.Thursday && !sprintVM.WorkThu)
                    continue;
                if (dataBase.DayOfWeek == DayOfWeek.Friday && !sprintVM.WorkFri)
                    continue;
                if (dataBase.DayOfWeek == DayOfWeek.Saturday && !sprintVM.WorkSat)
                    continue;

                var reuniaoDiaria = new Reuniao()
                {
                    TipoReuniao = TipoReuniao.Diaria,
                    Local = sprintVM.LocalRD,
                    Ata = "...",
                    DataInicial = dataBase,
                    DataFinal = dataBase.AddMinutes(15),
                    SprintId = sprintVM.Id
                };

                dailyScrum.Add(reuniaoDiaria);
            }

            if (dailyScrum.Count > 0)
            {
                _dbContext.Reunioes.AddRange(dailyScrum);
                _dbContext.SaveChanges();
            }
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
