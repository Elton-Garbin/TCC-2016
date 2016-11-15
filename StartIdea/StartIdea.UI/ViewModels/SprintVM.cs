using PagedList;
using StartIdea.Model.ScrumEventos;

namespace StartIdea.UI.ViewModels
{
    public class SprintVM
    {
        public IPagedList<Sprint> SprintList { get; set; }
        public Sprint SprintView { get; set; }
    }
}