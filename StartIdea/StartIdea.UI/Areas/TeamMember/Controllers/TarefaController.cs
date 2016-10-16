﻿using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.Areas.TeamMember.Models;
using StartIdea.UI.Areas.TeamMember.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.TeamMember.Controllers
{
    public class TarefaController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public TarefaController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index(string FiltroDescricao,
                                  string DisplayEdit,
                                  string DisplayCreate,
                                  int? PaginaGrid,
                                  int? IdEdit)
        {
            var tarefaVM = new TarefaVM();
            tarefaVM.FiltroDescricao = FiltroDescricao;
            tarefaVM.PaginaGrid = (PaginaGrid ?? 1);
            tarefaVM.DisplayCreate = DisplayCreate;
            tarefaVM.DisplayEdit = DisplayEdit;

            var sprintsBacklogs = from sb in _dbContext.SprintBacklogs.Include("ProductBacklog")
                                  where !sb.DataCancelamento.HasValue
                                  && (from sprint in _dbContext.Sprints
                                      where !sprint.DataCancelamento.HasValue
                                      && sprint.DataInicial <= DateTime.Now
                                      && sprint.DataFinal >= DateTime.Now
                                      select sprint.Id).Contains(sb.SprintId)
                                  orderby sb.ProductBacklog.Prioridade
                                  select sb;

            var tarefas = from t in _dbContext.Tarefas
                          join sb in _dbContext.SprintBacklogs
                          on t.SprintBacklogId equals sb.Id
                          join pb in _dbContext.ProductBacklogs
                          on sb.ProductBacklogId equals pb.Id
                          where (from sprint in _dbContext.Sprints
                                 where !sprint.DataCancelamento.HasValue
                                 && sprint.DataInicial <= DateTime.Now
                                 && sprint.DataFinal >= DateTime.Now
                                 select sprint.Id).Contains(sb.SprintId)
                          && !(from st in _dbContext.StatusTarefas
                               select st.TarefaId).Contains(t.Id)
                          && !sb.DataCancelamento.HasValue
                          orderby pb.Prioridade
                          select t;

            if (!string.IsNullOrEmpty(FiltroDescricao))
                tarefas = tarefas.Where(t => t.Descricao.Contains(FiltroDescricao));

            if ((IdEdit ?? 0) > 0)
            {
                Tarefa tarefa = _dbContext.Tarefas.Find(IdEdit);
                if (tarefa == null)
                    return HttpNotFound();

                tarefaVM.TarefaIdEdit    = tarefa.Id;
                tarefaVM.Descricao       = tarefa.Descricao;
                tarefaVM.SprintBacklogId = tarefa.SprintBacklogId;
                tarefaVM.DisplayEdit     = "Show";
            }

            tarefaVM.TarefaList = tarefas.ToPagedList(tarefaVM.PaginaGrid, 5);
            tarefaVM.sprintBacklogs = sprintsBacklogs.ToList();

            return View(tarefaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TarefaVM tarefaVM)
        {
            if (ModelState.IsValid)
            {
                Tarefa tarefa = new Tarefa()
                {
                    Descricao = tarefaVM.Descricao,
                    SprintBacklogId = tarefaVM.SprintBacklogId,
                    MembroTimeId = CurrentUser.TimeId
                };

                _dbContext.Tarefas.Add(tarefa);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Index", tarefaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TarefaVM tarefaVM)
        {
            if (ModelState.IsValid)
            {
                Tarefa tarefa = _dbContext.Tarefas.Find(tarefaVM.TarefaIdEdit);
                tarefa.Descricao       = tarefaVM.Descricao;
                tarefa.SprintBacklogId = tarefaVM.SprintBacklogId;

                _dbContext.Entry(tarefa).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return RedirectToAction("Index", new
                {
                    FiltroDescricao = tarefaVM.FiltroDescricao,
                    PaginaGrid = tarefaVM.PaginaGrid
                });
            }

            return RedirectToAction("Index", tarefaVM);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tarefa tarefa = _dbContext.Tarefas.Find(id);
            if (tarefa == null)
                return HttpNotFound();

            _dbContext.Tarefas.Remove(tarefa);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}