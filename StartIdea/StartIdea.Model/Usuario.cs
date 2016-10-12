using StartIdea.Model.TimeScrum;
using System.Collections.Generic;

namespace StartIdea.Model
{
    public class Usuario
    {
        public Usuario()
        {
            ProductOwners = new HashSet<ProductOwner>();
            ScrumMasters = new HashSet<ScrumMaster>();
            MembrosTime = new HashSet<MembroTime>();
        }

        #region Fields
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<ProductOwner> ProductOwners { get; set; }
        public virtual ICollection<ScrumMaster> ScrumMasters { get; set; }
        public virtual ICollection<MembroTime> MembrosTime { get; set; }
        #endregion
    }
}
