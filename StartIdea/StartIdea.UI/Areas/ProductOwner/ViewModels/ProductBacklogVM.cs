using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.ProductOwner.ViewModels
{
    public class ProductBacklogVM
    {
        public ProductBacklogVM()
        {
            Prioridade = 1;
        }

        #region Properties
        #region Filter
        public IPagedList<ProductBacklog> ProductBacklogList { get; set; }
        public string FiltroUserStory { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string FiltroDataInicial { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string FiltroDataFinal { get; set; }
        public int? PaginaGrid { get; set; }
        public string DisplayCreate { get; set; }
        public string DisplayEdit { get; set; }
        #endregion

        public int Id { get; set; }

        [DisplayName("User Story")]
        [Required(ErrorMessage = "Campo User Story obrigatório.")]
        [StringLength(150, ErrorMessage = "Campo User Story deve ter no máximo 150 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string UserStory { get; set; }

        [Required(ErrorMessage = "Campo Prioridade obrigatório.")]
        [Range(1, 9999, ErrorMessage = "Campo Prioridade deve ser maior do que zero.")]
        public short Prioridade { get; set; }
        #endregion
    }
}