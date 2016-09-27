using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StartIdea.UI.Areas.ProductOwner.ViewModels
{
    public class SprintVM : IDisposable
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public SprintVM()
        {
            Sprint sprint = GetSprintAtual();
            Id = sprint.Id;
            DataInicial = sprint.DataInicial;
            DataFinal = sprint.DataFinal;
            Objetivo = sprint.Objetivo;
            DataCadastro = sprint.DataCadastro;
            DataCancelamento = DateTime.Now;
        }

        #region Properties
        public int Id { get; private set; }
        public DateTime DataInicial { get; private set; }
        public DateTime DataFinal { get; private set; }
        public string Objetivo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime? DataCancelamento { get; private set; }

        [DisplayName("Motivo Cancelamento")]
        [Required(ErrorMessage = "Campo Motivo Cancelamento obrigatório.")]
        [DataType(DataType.MultilineText)]
        public string MotivoCancelamento { get; set; }
        #endregion

        #region Methods
        public Sprint GetSprintAtual()
        {
            return dbContext.Sprints.FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                       && s.TimeId == 1 // Remover
                                                       && s.DataInicial <= DateTime.Now
                                                       && s.DataFinal >= DateTime.Now) ?? new Sprint();
        }
        public void Dispose()
        {
            ((IDisposable)dbContext).Dispose();
        }
        #endregion
    }
}