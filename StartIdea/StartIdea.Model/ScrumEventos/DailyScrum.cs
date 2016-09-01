using System;

namespace StartIdea.Model.ScrumEventos
{
    public class DailyScrum
    {
        public DailyScrum()
        {

        }

        public int Id { get; set; }
        public string Local { get; set; }
        public DateTime DataHora { get; set; }
        public string Observacao { get; set; }
        public int SprintId { get; set; }

        public virtual Sprint Sprint { get; set; }
    }
}