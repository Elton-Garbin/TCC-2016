using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartIdea.UI.Areas.ProductOwner.ViewModels
{
    public class ProductBacklogCreateVM : ProductBacklog
    {
        public DateTime? filtroDataInicial { get; set; }
        public DateTime? filtroDataFinal { get; set; }
    }
}