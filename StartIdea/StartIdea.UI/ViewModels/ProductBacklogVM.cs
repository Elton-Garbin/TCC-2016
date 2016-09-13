using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;

namespace StartIdea.UI.ViewModels
{
    public class ProductBacklogVM : ProductBacklog
    {
        public IEnumerable<ProductBacklog> BackLogItem { get; set; }
    }
}