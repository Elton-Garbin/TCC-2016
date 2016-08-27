using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Time
{
    public class MembroTime
    {
        public MembroTime()
        {

        }

        public int Id { get; set; }
        public int TimeId { get; set; }
        public int UsuarioId { get; set; }
        public string Funcao { get; set; }
    }
}
