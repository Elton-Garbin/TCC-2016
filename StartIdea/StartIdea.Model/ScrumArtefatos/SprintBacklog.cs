using StartIdea.Model.ScrumEventos;
using System;
using System.Collections.Generic;

namespace StartIdea.Model.ScrumArtefatos
{
    public class SprintBacklog
    {
        public SprintBacklog()
        {
            Tarefas = new HashSet<Tarefa>();
        }

        public int Id { get; set; }
        public int SprintId { get; set; }
        public int ProductBacklogItemId { get; set; }

        public virtual Sprint Sprint { get; set; }
        public virtual ProductBacklogItem ProductBacklogItem { get; set; }

        public virtual ICollection<Tarefa> Tarefas { get; set; }
    }
}
