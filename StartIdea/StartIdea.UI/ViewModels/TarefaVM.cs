using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartIdea.UI.ViewModels
{
    public class TarefaVM
    {
        public int sprintId { get; set; }
        public int? productBacklogId { get; set; }
        public ProductBacklog productBacklog { get; set; }
        public List<Tarefa> tarefasBacklog { get; set; }
    }
}