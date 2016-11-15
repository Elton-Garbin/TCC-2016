using StartIdea.Model.ScrumArtefatos;
using System.Collections.Generic;

namespace StartIdea.UI.ViewModels
{
    public class TarefaVM
    {
        public List<Tarefa> TarefaList { get; set; }
        public ProductBacklog ProductBacklogView { get; set; }
        public Tarefa TarefaView { get; set; }

        public int SprintId { get; set; }
        public int? ProductBacklogId { get; set; }
    }
}