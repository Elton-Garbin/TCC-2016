using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class Status
    {
        public Status()
        {
            TarefaStatus = new HashSet<TarefaStatus>();
        }

        public int Id { get; set; }
        //[Required]
        //[StringLength(20)]
        public string Descricao { get; set; }
        public bool? Ready { get; set; }
        public bool? InProgress { get; set; }
        public bool? Done { get; set; }

        public virtual ICollection<TarefaStatus> TarefaStatus { get; set; }
    }
}