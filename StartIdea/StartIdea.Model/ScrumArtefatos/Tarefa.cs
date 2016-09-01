using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class Tarefa
    {
        public Tarefa()
        {
            TarefasStatus = new HashSet<TarefaStatus>();
        }

        public int Id { get; set; }
        public int SprintBacklogId { get; set; }
        public string Descricao { get; set; }

        public virtual SprintBacklog SprintBacklog { get; set; }

        public virtual ICollection<TarefaStatus> TarefasStatus { get; set; }
    }
}