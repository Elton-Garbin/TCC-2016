using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Artefatos
{
    public class TarefaStatus
    {
        public TarefaStatus()
        {

        }

        public int Id { get; set; }
        public int TarefaId { get; set; }
        public int StatusId { get; set; }
        public int MembroTimeId { get; set; }
    }
}
