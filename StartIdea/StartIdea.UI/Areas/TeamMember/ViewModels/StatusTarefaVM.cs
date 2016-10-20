using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartIdea.UI.Areas.TeamMember.ViewModels
{
    public class StatusTarefaVM
    {
        public IEnumerable<Status> StatusProcesso { get; set; }
        public IEnumerable<Tarefa> TarefasPendentes { get; set; }
    }
}