using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Artefatos
{
    public class SprintBacklog
    {
        public SprintBacklog()
        {

        }

        public int Id { get; set; }
        public int SprintId { get; set; }
        public int ProductBacklogItem { get; set; }
    }
}
