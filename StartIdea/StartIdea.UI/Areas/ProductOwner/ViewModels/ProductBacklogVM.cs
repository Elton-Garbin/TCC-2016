using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;

namespace StartIdea.UI.Areas.ProductOwner.ViewModels
{
    public class ProductBacklogVM : ProductBacklog
    {
        public IPagedList<ProductBacklog> ProductBacklogList { get; set; }
        public ProductBacklog ProductBacklogEdit { get; set; }
        public ProductBacklogCreateVM ProductBacklogCreateVM { get; set; }
        public DateTime? filtroDataInicial { get; set; }
        public DateTime? filtroDataFinal { get; set; }
        public string DisplayEdit { get; set; }
    }
}