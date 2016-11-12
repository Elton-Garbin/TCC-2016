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

        #region Fields
        public int Id { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public string MotivoCancelamento { get; set; }
        public int SprintId { get; set; }
        public int ProductBacklogId { get; set; }
        #endregion

        #region References
        public virtual Sprint Sprint { get; set; }
        public virtual ProductBacklog ProductBacklog { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Tarefa> Tarefas { get; set; }
        #endregion
    }
}
