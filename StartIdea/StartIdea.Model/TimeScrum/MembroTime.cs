using StartIdea.Model.ScrumArtefatos;
using System.Collections.Generic;

namespace StartIdea.Model.TimeScrum
{
    public class MembroTime
    {
        public MembroTime()
        {
            InteracoesProductBacklogItem = new HashSet<InteracaoProductBacklogItem>();
            Tarefas = new HashSet<Tarefa>();
            //TarefasStatus = new HashSet<TarefaStatus>();
        }

        public int Id { get; set; }
        public string Funcao { get; set; }
        public int TimeId { get; set; }
        public int UsuarioId { get; set; }

        public virtual Time Time { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<InteracaoProductBacklogItem> InteracoesProductBacklogItem { get; set; }
        public virtual ICollection<Tarefa> Tarefas { get; set; }
        //public virtual ICollection<TarefaStatus> TarefasStatus { get; set; }
    }
}
