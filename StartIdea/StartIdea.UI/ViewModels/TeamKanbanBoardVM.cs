using StartIdea.Model.ScrumArtefatos;
using System.Collections.Generic;

namespace StartIdea.UI.ViewModels
{
    public class TeamKanbanBoardVM
    {
        public int SprintId { get; set; }
        public IEnumerable<Status> StatusProcesso { get; set; }
        public IEnumerable<Tarefa> Tarefas { get; set; }
    }
}