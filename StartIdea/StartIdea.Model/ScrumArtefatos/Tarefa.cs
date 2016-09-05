using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class Tarefa
    {
        public Tarefa()
        {
            StatusTarefas = new HashSet<StatusTarefa>();

            DataInclusao = DateTime.Now;
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInclusao { get; set; }
        public int SprintBacklogId { get; set; }
        public int MembroTimeId { get; set; }

        public virtual SprintBacklog SprintBacklog { get; set; }
        public virtual MembroTime MembroTime { get; set; }

        public virtual ICollection<StatusTarefa> StatusTarefas { get; set; }
    }
}