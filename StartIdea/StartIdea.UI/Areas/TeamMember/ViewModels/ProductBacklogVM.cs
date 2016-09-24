using PagedList;
using StartIdea.Model.ScrumArtefatos;

namespace StartIdea.UI.Areas.TeamMember.ViewModels
{
    public class ProductBacklogVM
    {
        public IPagedList<ProductBacklog> ProductBacklogList { get; set; }
        public ProductBacklog ProductBacklogEdit { get; set; }
        public string DisplayEdit { get; set; }
    }
}