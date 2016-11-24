using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System.Collections.Generic;
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
        public IEnumerable<ProductBacklog> ProductBacklogReport { get; set; }
        public string FiltroUserStory { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string FiltroDataInicial { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string FiltroDataFinal { get; set; }
        public int? PaginaGrid { get; set; }
        #endregion

        public int Id { get; set; }

        [DisplayName("User Story")]
        [Required(ErrorMessage = "Campo User Story obrigatório.")]
        [StringLength(150, ErrorMessage = "Campo User Story deve ter no máximo 150 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string UserStory { get; set; }

        [Required(ErrorMessage = "Campo Prioridade obrigatório.")]
        [Range(1, 9999, ErrorMessage = "Campo Prioridade deve ser maior do que zero.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Informe um número válido.")]
        public short Prioridade { get; set; }
        #endregion
    }
}