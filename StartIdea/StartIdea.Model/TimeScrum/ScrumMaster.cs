using System.Collections.Generic;

namespace StartIdea.Model.TimeScrum
{
    public class ScrumMaster
    {
        public ScrumMaster()
        {
            Time = new HashSet<Time>();
        }

        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Time> Time { get; set; }
    }
}