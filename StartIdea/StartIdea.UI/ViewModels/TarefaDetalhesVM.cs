using StartIdea.Model.ScrumArtefatos;

namespace StartIdea.UI.ViewModels
{
    public class TarefaDetalhesVM
    {
        public int sprintId { get; set; }
        public int productBacklogId { get; set; }
        public Tarefa tarefa { get; set; }
    }
}