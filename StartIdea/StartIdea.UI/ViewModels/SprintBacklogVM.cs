using StartIdea.Model.ScrumEventos;
using System;
using System.Collections.Generic;

namespace StartIdea.UI.ViewModels
{
    public class SprintBacklogVM
    {
        public IEnumerable<Sprint> Sprints { get; set; }
    }
}