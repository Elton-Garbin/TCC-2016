using StartIdea.Model.ScrumEventos;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using StartIdea.DataAccess;
using System.Linq;

namespace StartIdea.UI.Areas.ScrumMaster.ViewModels
{
    public class SprintVM : IDisposable, IValidatableObject
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public SprintVM()
        {
            GetScrumMasterTeamId();
            GetSprintAtual();
            GetProximaSprint();
        }

        #region Properties
        public int TimeId { get; private set; }
        public Sprint SprintAtual { get; private set; }
        public string ActionForm { get; private set; }
        public string SubmitValue { get; private set; }

        #region ProximaSprint
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;

                SubmitValue = (_Id > 0) ? "Editar" : "Cadastrar";
                ActionForm = (_Id > 0) ? "Edit" : "Create";
            }
        }

        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "Campo Data Inicial obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "Campo Data Inicial inválido.")]
        public DateTime DataInicial { get; set; }

        [DisplayName("Data Final")]
        [Required(ErrorMessage = "Campo Data Final obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "Campo Data Final inválido.")]
        public DateTime DataFinal { get; set; }

        [Required(ErrorMessage = "Campo Objetivo obrigatório.")]
        [StringLength(75, ErrorMessage = "Campo User Story deve ter no máximo 75 caracteres.")]
        public string Objetivo { get; set; }
        #endregion
        #endregion

        #region Methods
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SprintAtual != null)
            {
                if (DataInicial <= SprintAtual.DataFinal)
                {
                    yield return
                        new ValidationResult(errorMessage: "Data Inicial deve ser maior do que a data final da sprint atual.",
                                             memberNames: new[] { "DataInicial" });
                }
            }

            if (DataFinal <= DataInicial)
            {
                yield return
                    new ValidationResult(errorMessage: "Data Final deve ser maior do que a Data Inicial.",
                                         memberNames: new[] { "DataFinal" });
            }
        }

        private void GetScrumMasterTeamId()
        {
            TimeId = dbContext.Times.Where(t => t.ScrumMasterId == 1).FirstOrDefault().Id;
        }

        private void GetSprintAtual()
        {
            SprintAtual = dbContext.Sprints.FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                              && s.TimeId == TimeId
                                                              && s.DataInicial <= DateTime.Now
                                                              && s.DataFinal >= DateTime.Now);
        }

        private void GetProximaSprint()
        {
            Sprint sprint =  dbContext.Sprints.Where(s => !s.DataCancelamento.HasValue
                                                        && s.TimeId == TimeId
                                                        && s.DataInicial > DateTime.Now)
                                              .OrderByDescending(s => s.DataInicial)
                                              .FirstOrDefault() ?? new Sprint();

            Id = sprint.Id;
            Objetivo = sprint.Objetivo;
            DataInicial = sprint.DataInicial;
            DataFinal = sprint.DataFinal;
        }

        public void Dispose()
        {
            ((IDisposable)dbContext).Dispose();
        }
        #endregion
    }
}