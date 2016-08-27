using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartIdea.Model.Scrum.Time
{
    public class Time
    {
        public Time()
        {

        }

        public int Id { get; set; }
        public int ScrumMasterId { get; set; }
        public string Nome { get; set; }
    }
}
