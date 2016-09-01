using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class Status
    {
        public Status()
        {
            TarefasStatus = new HashSet<TarefaStatus>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool? Ready { get; set; }
        public bool? InProgress { get; set; }
        public bool? Done { get; set; }

        public virtual ICollection<TarefaStatus> TarefasStatus { get; set; }
    }
}