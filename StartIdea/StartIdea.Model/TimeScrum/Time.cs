using StartIdea.Model.ScrumEventos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.Model.TimeScrum
{
    public class Time
    {
        public Time()
        {
            ScrumMasters = new HashSet<ScrumMaster>();
            MembrosTime = new HashSet<MembroTime>();
            Sprints = new HashSet<Sprint>();
        }

        #region Fields
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Nome { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<ScrumMaster> ScrumMasters { get; set; }
        public virtual ICollection<MembroTime> MembrosTime { get; set; }
        public virtual ICollection<Sprint> Sprints { get; set; }
        #endregion
    }
}