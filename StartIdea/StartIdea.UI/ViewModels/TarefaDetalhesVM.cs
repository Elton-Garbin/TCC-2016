using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartIdea.UI.ViewModels
{
    public class TarefaDetalhesVM
    {
        public int sprintId { get; set; }
        public int productBacklogId { get; set; }
        public Tarefa tarefa { get; set; }
    }
}