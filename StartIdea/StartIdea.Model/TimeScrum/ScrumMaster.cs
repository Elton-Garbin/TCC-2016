using System.Collections.Generic;

namespace StartIdea.Model.TimeScrum
{
    public class ScrumMaster
    {
        public ScrumMaster()
        {
            Times = new HashSet<Time>();
        }

        #region Fields
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        #endregion

        #region References
        public virtual Usuario Usuario { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Time> Times { get; set; }
        #endregion
    }
}
