using StartIdea.Model.TimeScrum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.Model.ScrumArtefatos
{
    public enum StoryPoint : int
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
            DataInclusao = DateTime.Now;
            StoryPoint = StoryPoint.N;
            Prioridade = 1;
        }

        #region Fields
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string UserStory { get; set; }

        public StoryPoint StoryPoint { get; set; }

        [Required]
        [Range(0, 9999)]
        public short Prioridade { get; set; }

        public DateTime DataInclusao { get; set; }
        public int ProductOwnerId { get; set; }
        #endregion

        #region References
        public virtual ProductOwner ProductOwner { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<HistoricoEstimativa> HistoricoEstimativas { get; set; }
        public virtual ICollection<SprintBacklog> SprintBacklogs { get; set; }
        #endregion
    }
}
