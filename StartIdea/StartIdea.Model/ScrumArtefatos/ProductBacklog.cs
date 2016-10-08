using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.Model.ScrumArtefatos
{
    public enum StoryPoint
    {
        N = 0,
        PP = 1,
        P = 3,
        M = 5,
        G = 8,
        GG = 13
    }

    public class ProductBacklog
    {
        public ProductBacklog()
        {
            HistoricoEstimativas = new HashSet<HistoricoEstimativa>();
            SprintBacklogs = new HashSet<SprintBacklog>();

            #region Default Values
            DataInclusao = DateTime.Now;
            StoryPoint = StoryPoint.N;
            Prioridade = 1;
            #endregion
        }

        #region Campos
        public int Id { get; set; }

        [DisplayName("User Story")]
        [Required(ErrorMessage = "Campo User Story obrigatório.")]
        [StringLength(150, ErrorMessage = "Campo User Story deve ter no máximo 150 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string UserStory { get; set; }

        [DisplayName("Story Point")]
        public StoryPoint StoryPoint { get; set; }

        [Required(ErrorMessage = "Campo Prioridade obrigatório.")]
        [Range(0, 9999, ErrorMessage = "Campo Prioridade deve ser maior do que zero.")]
        public short Prioridade { get; set; }

        [DisplayName("Data Inclusão")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInclusao { get; set; }

        public int ProductOwnerId { get; set; }
        #endregion

        public virtual ProductOwner ProductOwner { get; set; }

        public virtual ICollection<HistoricoEstimativa> HistoricoEstimativas { get; set; }
        public virtual ICollection<SprintBacklog> SprintBacklogs { get; set; }
    }
}
