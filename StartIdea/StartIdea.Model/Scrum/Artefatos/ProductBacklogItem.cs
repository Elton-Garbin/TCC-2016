using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Artefatos
{
    public class ProductBacklogItem
    {
        public ProductBacklogItem()
        {

        }

        public int Id { get; set; }
        public int ProductBacklogId { get; set; }
        public string UserStory { get; set; }
        public string Tamanho { get; set; }
        public int Prioridade { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataExclusao { get; set; }
    }
}
