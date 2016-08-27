using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Artefatos
{
    public class Tarefa
    {
        public Tarefa()
        {

        }

        public int Id { get; set; }
        public int SprintBacklogId { get; set; }
        public string Descricao { get; set; }
    }
}
