using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System;
using System.ComponentModel;

namespace StartIdea.UI.Areas.TeamMember.ViewModels
{
    public class ProductBacklogVM
    {
        public IPagedList<ProductBacklog> ProductBacklogList { get; set; }
        public string FiltroUserStory { get; set; }

        public int Id { get; set; }

        [DisplayName("User Story")]
        public string UserStory { get; set; }
        public StartIdea.Model.TimeScrum.ProductOwner ProductOwner { get; set; }
        public int Prioridade { get; set; }
        [DisplayName("Data Inclusão")]
        public DateTime DataInclusao { get; set; }

        [DisplayName("Story Point")]
        public StoryPoint StoryPoint { get; set; }

        public string DisplayEdit { get; set; }
    }
}