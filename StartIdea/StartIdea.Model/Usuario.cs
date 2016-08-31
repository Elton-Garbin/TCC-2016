using StartIdea.Model.TimeScrum;
using System.Collections.Generic;

namespace StartIdea.Model
{
    public class Usuario
    {
        public Usuario()
        {
            MembroTime = new HashSet<MembroTime>();
            ProductOwner = new HashSet<ProductOwner>();
            ScrumMaster = new HashSet<ScrumMaster>();
        }

        public int Id { get; set; }
        //[Required]
        //[StringLength(20)]
        public string UserName { get; set; }
        //[Required]
        //[StringLength(100)]
        public string Email { get; set; }
        //[Required]
        public byte[] Senha { get; set; }
        //[StringLength(100)]
        public string Nome { get; set; }

        public virtual ICollection<MembroTime> MembroTime { get; set; }
        public virtual ICollection<ProductOwner> ProductOwner { get; set; }
        public virtual ICollection<ScrumMaster> ScrumMaster { get; set; }
    }
}