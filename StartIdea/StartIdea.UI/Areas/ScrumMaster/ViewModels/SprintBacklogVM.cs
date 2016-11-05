using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.ScrumMaster.ViewModels
{
    public class SprintBacklogVM
    {
        public int SprintId { get; set; }
        public int PaginaGridProductBacklog { get; set; }
        public IPagedList<ProductBacklog> ProductBacklogList { get; set; }
        public int PaginaGridSprintBacklog { get; set; }
        public IPagedList<ProductBacklog> SprintBacklogList { get; set; }
        public string DisplayMotivoCancelamento { get; set; }
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Motivo Cancelamento")]
        public string MotivoCancelamento { get; set; }
    }
}