using PagedList;
using StartIdea.Model.ScrumArtefatos;

namespace StartIdea.UI.Areas.ProductOwner.ViewModels
{
    public class ProductBacklogVM : ProductBacklog
    {
        public IPagedList<ProductBacklog> productBacklogs { get; set; }
        public ProductBacklog productBacklogEdit { get; set; }
        public ProductBacklog productBacklogCreate { get; set; }
        public string DisplayEdit { get; set; }
        public string DisplayCreate { get; set; }
    }
}