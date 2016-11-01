using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartIdea.UI.ViewModels
{
    public class DetalheProductBacklogVM
    {
        public ProductBacklog productBacklog { get; set; }
        public int? sprintId { get; set; }
    }
}