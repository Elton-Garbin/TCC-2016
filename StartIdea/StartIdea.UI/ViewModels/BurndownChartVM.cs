using StartIdea.Model.ScrumEventos;
using StartIdea.UI.Models;
using System.Collections.Generic;

namespace StartIdea.UI.ViewModels
{
    public class BurndownChartVM
    {

        public Sprint SprintAtual { get; set; }
        public string[] Labels { get; set; }
        public List<ChartDatasetFacade> Datasets { get; set; }
    }
}