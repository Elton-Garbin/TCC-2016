using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.Model.ScrumArtefatos
{
    public class Tarefa
    {
        public Tarefa()
        {
            StatusTarefas = new HashSet<StatusTarefa>();
            DataInclusao = DateTime.Now;
        }

        #region Fields
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }

        [DisplayName("Data Inclusão"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInclusao { get; set; }

        public int SprintBacklogId { get; set; }
        public int MembroTimeId { get; set; }
        #endregion

        public DateTime? DataCancelamento { get; set; }
        public string MotivoCancelamento { get; set; }

        #region References
        public virtual SprintBacklog SprintBacklog { get; set; }
        public virtual MembroTime MembroTime { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<StatusTarefa> StatusTarefas { get; set; }
        #endregion
    }
}