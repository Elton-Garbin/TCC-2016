using System.Collections.Generic; using System.ComponentModel.DataAnnotations;

namespace StartIdea.Model.ScrumArtefatos {     public enum Classificacao     {         Available = 0,         Ready = 1,         InProgress = 2,         Done = 3     }      public class Status     {         public Status()         {             StatusTarefas = new HashSet<StatusTarefa>();         }

        #region Fields
        public int Id { get; set; }

        [Required, MaxLength(20)]         public string Descricao { get; set; }         public Classificacao Classificacao { get; set; }
        #endregion 
        #region Collections
        public virtual ICollection<StatusTarefa> StatusTarefas { get; set; }
        #endregion
    } }