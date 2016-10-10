using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StartIdea.UI.Areas.ScrumMaster.ViewModels
{
    public class SprintBacklogVM
    {
        public int paginaProductBacklog { get; set; }
        public IPagedList<ProductBacklog> ProductBacklog { get; set; }

        public int paginaSprintBacklog { get; set; }
        public IPagedList<ProductBacklog> SprintBacklog { get; set; }

        public string DisplayMotivoCancelamento { get; set; }
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Motivo Cancelamento")]
        public string MotivoCancelamento { get; set; }
    }
}