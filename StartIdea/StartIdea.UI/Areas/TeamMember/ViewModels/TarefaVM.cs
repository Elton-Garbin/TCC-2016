using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StartIdea.UI.Areas.TeamMember.ViewModels
{
    public class TarefaVM
    {
        public IPagedList<Tarefa> TarefaList { get; set; }
        public string FiltroDescricao { get; set; }
        public int PaginaGrid { get; set; }
        public string DisplayCreate { get; set; }

        [DisplayName("Backlog da Sprint")]
        public IEnumerable<SprintBacklog> sprintBacklogs { get; set; }

        [DisplayName("Descrição")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        public int SprintBacklogIdInsert { get; set; }
    }
}