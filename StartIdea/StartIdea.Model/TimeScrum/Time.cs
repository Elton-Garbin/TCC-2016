using StartIdea.Model.ScrumEventos;
using System.Collections.Generic;

namespace StartIdea.Model.TimeScrum
{
    public class Time
    {
        public Time()
        {
            MembrosTime = new HashSet<MembroTime>();
            Sprints = new HashSet<Sprint>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int ScrumMasterId { get; set; }

        public virtual ScrumMaster ScrumMaster { get; set; }

        public virtual ICollection<MembroTime> MembrosTime { get; set; }
        public virtual ICollection<Sprint> Sprints { get; set; }
    }
}