using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class Tarefa
    {
        public Tarefa()
        {
            TarefaStatus = new HashSet<TarefaStatus>();
        }

        public int Id { get; set; }
        public int SprintBacklogId { get; set; }
        //[Required]
        //[StringLength(100)]
        public string Descricao { get; set; }

        public virtual SprintBacklog SprintBacklog { get; set; }

        public virtual ICollection<TarefaStatus> TarefaStatus { get; set; }
    }
}