using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Artefatos
{
    public class ProductBacklog
    {
        public ProductBacklog()
        {

        }

        public int Id { get; set; }
        public int ProductOwnerId { get; set; }
        public string Descricao { get; set; }
    }
}
