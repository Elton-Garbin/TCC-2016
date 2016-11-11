using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.ScrumMaster.ViewModels
{
    public class SprintBacklogVM
    {
        public int PaginaGridProductBacklog { get; set; }
        public IPagedList<ProductBacklog> ProductBacklogList { get; set; }
        public int PaginaGridSprintBacklog { get; set; }
        public IPagedList<SprintBacklog> SprintBacklogList { get; set; }
        public string DisplayMotivoCancelamento { get; set; }
        public IEnumerable<SprintBacklog> SprintBacklogReport { get; set; }

        public int Id { get; set; }
        public int SprintId { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Motivo Cancelamento")]
        public string MotivoCancelamento { get; set; }
    }
}