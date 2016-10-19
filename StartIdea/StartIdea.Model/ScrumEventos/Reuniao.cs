using System;
using System.ComponentModel.DataAnnotations;

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
        #region Fields
        public int Id { get; set; }
        public TipoReuniao TipoReuniao { get; set; }

        [Required, MaxLength(50)]
        public string Local { get; set; }

        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }

        [Required]
        public string Ata { get; set; }

        public int SprintId { get; set; }
        #endregion

        #region References
        public virtual Sprint Sprint { get; set; }
        #endregion
    }
}