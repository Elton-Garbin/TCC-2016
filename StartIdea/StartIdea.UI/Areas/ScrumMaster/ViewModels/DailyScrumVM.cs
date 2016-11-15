using PagedList;
using StartIdea.Model.ScrumEventos;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.ScrumMaster.ViewModels
{
    public class DailyScrumVM
    {
        public DailyScrumVM()
        {
            DataInicial = DateTime.Now;
        }

        #region Properties
        #region Filter
        public IPagedList<Reuniao> ReuniaoList { get; set; }
        public int PaginaGrid { get; set; }
        #endregion

        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Local obrigatório.")]
        [StringLength(50, ErrorMessage = "Campo Local deve ter no máximo 50 caracteres.")]
        public string Local { get; set; }

        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "Campo Data Inicial obrigatório.")]
        public DateTime DataInicial { get; set; }

        [Required(ErrorMessage = "Campo Ata obrigatório.")]
        [DataType(DataType.MultilineText)]
        public string Ata { get; set; }

        public int SprintId { get; set; }
        #endregion
    }
}