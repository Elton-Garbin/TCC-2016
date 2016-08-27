using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Artefatos
{
    public class Status
    {
        public Status()
        {

        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ready { get; set; }
        public bool InProgress { get; set; }
        public bool Done { get; set; }
    }
}
