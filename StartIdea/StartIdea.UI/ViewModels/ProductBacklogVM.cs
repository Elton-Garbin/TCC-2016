using PagedList;
using StartIdea.Model.ScrumArtefatos;

namespace StartIdea.UI.ViewModels
{
    public class ProductBacklogVM
    {
        public StoryPoint? StoryPoint { get; set; }
        public string UserStory { get; set; }
        public int? SprintId { get; set; }
        public IPagedList<ProductBacklog> ProductBacklogList { get; set; }

        public ProductBacklog ProductBacklogView { get; set; }
    }
}