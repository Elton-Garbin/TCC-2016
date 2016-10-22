using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.Model
{
    public class Usuario
    {
        public Usuario()
        {
            ProductOwners = new HashSet<ProductOwner>();
            ScrumMasters = new HashSet<ScrumMaster>();
            MembrosTime = new HashSet<MembroTime>();
            DataInclusao = DateTime.Now;
            IsActive = true;
            IsAdmin = false;
        }

        #region Fields
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required, MaxLength(256)]
        public string UserName { get; set; }

        [Required, StringLength(11, MinimumLength = 11)]
        public string CPF { get; set; }

        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool IsActive { get; set; }
        public Guid? TokenActivation { get; set; }
        public bool IsAdmin { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<ProductOwner> ProductOwners { get; set; }
        public virtual ICollection<ScrumMaster> ScrumMasters { get; set; }
        public virtual ICollection<MembroTime> MembrosTime { get; set; }
        #endregion
    }
}
