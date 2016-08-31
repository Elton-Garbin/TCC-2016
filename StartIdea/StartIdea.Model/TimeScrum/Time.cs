using StartIdea.Model.ScrumEventos;
using System.Collections.Generic;

namespace StartIdea.Model.TimeScrum
{
    public class Time
    {
        public Time()
        {
            MembroTime = new HashSet<MembroTime>();
            Sprint = new HashSet<Sprint>();
        }

        public int Id { get; set; }
        //[Required]
        //[StringLength(50)]
        public string Nome { get; set; }
        public int ScrumMasterId { get; set; }

        public virtual ScrumMaster ScrumMaster { get; set; }

        public virtual ICollection<MembroTime> MembroTime { get; set; }
        public virtual ICollection<Sprint> Sprint { get; set; }
    }
}