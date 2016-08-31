using System;

namespace StartIdea.Model.ScrumEventos
{
    public class RevisaoSprint
    {
        public RevisaoSprint()
        {

        }

        public int Id { get; set; }
        //[Required]
        //[StringLength(50)]
        public string Local { get; set; }
        public DateTime DataHora { get; set; }
        public string Observacao { get; set; }
        public int SprintId { get; set; }

        public virtual Sprint Sprint { get; set; }
    }
}