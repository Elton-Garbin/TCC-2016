using System;

namespace StartIdea.Model.TimeScrum
{
    public class ScrumMaster
    {
        public ScrumMaster()
        {
            IsActive = true;
            DataManutencao = DateTime.Now;
        }

        #region Fields
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime DataManutencao { get; set; }
        public int UsuarioId { get; set; }
        public int TimeId { get; set; }
        #endregion

        #region References
        public virtual Usuario Usuario { get; set; }
        public virtual Time Time { get; set; }
        #endregion
    }
}
