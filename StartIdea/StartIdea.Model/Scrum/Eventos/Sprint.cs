using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Eventos
{
    public class Sprint
    {
        public Sprint()
        {

        }

        public int Id { get; set; }
        public int TimeId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Objetivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataCancelamento { get; set; }
    }
}
