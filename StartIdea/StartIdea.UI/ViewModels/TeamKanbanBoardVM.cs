using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartIdea.UI.ViewModels
{
    public class TeamKanbanBoardVM
    {
        public IEnumerable<Status> StatusProcesso { get; set; }
        public IEnumerable<Tarefa> Tarefas { get; set; }
    }
}