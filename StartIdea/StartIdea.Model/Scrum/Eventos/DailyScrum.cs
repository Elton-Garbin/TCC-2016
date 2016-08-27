using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Eventos
{
    public class DailyScrum
    {
        public DailyScrum()
        {

        }

        public int Id { get; set; }
        public int SprintId { get; set; }
        public string Local { get; set; }
        public DateTime DataHora { get; set; }
        public string Observacao { get; set; }
    }
}
