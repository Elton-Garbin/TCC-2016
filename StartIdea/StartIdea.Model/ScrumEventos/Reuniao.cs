using System;

namespace StartIdea.Model.ScrumEventos
{
    public enum TipoReuniao
    {
        Planejamento = 1,
        Diaria = 2,
        Revisao = 3,
        Retrospectiva = 4
    }

    public class Reuniao
    {
        public int Id { get; set; }
        public TipoReuniao TipoReuniao { get; set; }
        public string Local { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Observacao { get; set; }
        public int SprintId { get; set; }

        public virtual Sprint Sprint { get; set; }
    }
}