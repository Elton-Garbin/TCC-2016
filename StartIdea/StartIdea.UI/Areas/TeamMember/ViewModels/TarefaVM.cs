using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.TeamMember.ViewModels
{
    public class TarefaVM
    {
        #region Properties
        #region Filter
        public IPagedList<Tarefa> TarefaList { get; set; }
        public string FiltroDescricao { get; set; }
        public int PaginaGrid { get; set; }
        public string DisplayMotivoCancelamento { get; set; }
        public int? TarefaIdCancelamento { get; set; }
        public int SprintId { get; set; }
        #endregion
        
        public int Id { get; set; }

        [DisplayName("Descrição")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Campo Descrição obrigatório.")]
        public string Descricao { get; set; }

        public int SprintBacklogId { get; set; }

        [DisplayName("Backlog da Sprint")]
        public IEnumerable<SprintBacklog> SprintBacklogs { get; set; }

        [DisplayName("Motivo do Cancelamento")]
        [DataType(DataType.MultilineText)]
        public string MotivoCancelamento { get; set; }
        #endregion
    }
}