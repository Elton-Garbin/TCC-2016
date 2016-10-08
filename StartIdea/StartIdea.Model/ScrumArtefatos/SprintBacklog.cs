using StartIdea.Model.ScrumEventos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public int ProductBacklogId { get; set; }

        public DateTime? DataCancelamento { get; set; }

        [DataType(DataType.MultilineText)]
        public string MotivoCancelamento { get; set; }

        public virtual Sprint Sprint { get; set; }
        public virtual ProductBacklog ProductBacklog { get; set; }

        public virtual ICollection<Tarefa> Tarefas { get; set; }
    }
}
