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

            DataInclusao = DateTime.Now;
            StoryPoint = StoryPoint.N;
        }

        public int Id { get; set; }
        [DisplayName("User Story"), Required, StringLength(150), DataType(DataType.MultilineText)]
        public string UserStory { get; set; }
        [DisplayName("Story Point")]
        public StoryPoint StoryPoint { get; set; }
        public short Prioridade { get; set; }
        [DisplayName("Data Inclusão"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInclusao { get; set; }
        public int ProductOwnerId { get; set; }

        public virtual ProductOwner ProductOwner { get; set; }

        public virtual ICollection<HistoricoEstimativa> HistoricoEstimativas { get; set; }
        public virtual ICollection<SprintBacklog> SprintBacklogs { get; set; }
    }
}
