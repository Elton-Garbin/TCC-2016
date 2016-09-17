using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartIdea.UI.Areas.ProductOwner.ViewModel
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