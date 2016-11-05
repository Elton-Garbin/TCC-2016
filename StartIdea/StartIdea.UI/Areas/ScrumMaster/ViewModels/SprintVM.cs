using StartIdea.Model.ScrumEventos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.UI.Areas.ScrumMaster.ViewModels
{
    public class SprintVM : IValidatableObject
    {
        public SprintVM()
        {
            Id = 0;
            HorarioInicialRD = "08:00";
            WorkMon = true;
            WorkTue = true;
            WorkWed = true;
            WorkThu = true;
            WorkFri = true;
        }

        #region Properties
        public Sprint SprintAtual { get; set; }
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
        [Required(ErrorMessage = "[Sprint] Campo Data Inicial obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "[Sprint] Campo Data Inicial inválido.")]
        public DateTime DataInicial { get; set; }

        [DisplayName("Data Final")]
        [Required(ErrorMessage = "[Sprint] Campo Data Final obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "[Sprint] Campo Data Final inválido.")]
        public DateTime DataFinal { get; set; }

        [Required(ErrorMessage = "[Sprint] Campo Objetivo obrigatório.")]
        [StringLength(75, ErrorMessage = "[Sprint] Campo User Story deve ter no máximo 75 caracteres.")]
        public string Objetivo { get; set; }

        #region Reunião Planejamento
        public int IdRP { get; set; }

        [DisplayName("Local")]
        [Required(ErrorMessage = "[Reunião Planejamento] Campo Local obrigatório.")]
        [StringLength(50, ErrorMessage = "[Reunião Planejamento] Campo Local deve ter no máximo 50 caracteres.")]
        public string LocalRP { get; set; }

        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "[Reunião Planejamento] Campo Data Inicial obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "[Reunião Planejamento] Campo Data Inicial inválido.")]
        public DateTime DataInicialRP { get; set; }

        [DisplayName("Data Final")]
        [Required(ErrorMessage = "[Reunião Planejamento] Campo Data Final obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "[Reunião Planejamento] Campo Data Final inválido.")]
        public DateTime DataFinalRP { get; set; }
        #endregion

        #region Reunião Diária
        [DisplayName("Local")]
        [Required(ErrorMessage = "[Reunião Diária] Campo Local obrigatório.")]
        [StringLength(50, ErrorMessage = "[Reunião Diária] Campo Local deve ter no máximo 50 caracteres.")]
        public string LocalRD { get; set; }

        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "[Reunião Diária] Campo Data Inicial obrigatório.")]
        [DataType(DataType.Date, ErrorMessage = "[Reunião Diária] Campo Data Inicial inválido.")]
        public DateTime DataInicialRD { get; set; }

        [DisplayName("Data Final")]
        [Required(ErrorMessage = "[Reunião Diária] Campo Data Final obrigatório.")]
        [DataType(DataType.Date, ErrorMessage = "[Reunião Diária] Campo Data Final inválido.")]
        public DateTime DataFinalRD { get; set; }

        [DisplayName("Horário Inicial")]
        [Required(ErrorMessage = "[Reunião Diária] Campo Horário Inicial obrigatório.")]
        [DataType(DataType.Time, ErrorMessage = "[Reunião Diária] Campo Horário Inicial inválido.")]
        public string HorarioInicialRD { get; set; }

        public bool WorkSun { get; set; }
        public bool WorkMon { get; set; }
        public bool WorkTue { get; set; }
        public bool WorkWed { get; set; }
        public bool WorkThu { get; set; }
        public bool WorkFri { get; set; }
        public bool WorkSat { get; set; }
        #endregion

        #region Reunião Revisão
        public int IdRV { get; set; }

        [DisplayName("Local")]
        [Required(ErrorMessage = "[Reunião Revisão] Campo Local obrigatório.")]
        [StringLength(50, ErrorMessage = "[Reunião Revisão] Campo Local deve ter no máximo 50 caracteres.")]
        public string LocalRV { get; set; }

        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "[Reunião Revisão] Campo Data Inicial obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "[Reunião Revisão] Campo Data Inicial inválido.")]
        public DateTime DataInicialRV { get; set; }

        [DisplayName("Data Final")]
        [Required(ErrorMessage = "[Reunião Revisão] Campo Data Final obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "[Reunião Revisão] Campo Data Final inválido.")]
        public DateTime DataFinalRV { get; set; }
        #endregion

        #region Reunião Retrospectiva
        public int IdRT { get; set; }

        [DisplayName("Local")]
        [Required(ErrorMessage = "[Reunião Retrospectiva] Campo Local obrigatório.")]
        [StringLength(50, ErrorMessage = "[Reunião Retrospectiva] Campo Local deve ter no máximo 50 caracteres.")]
        public string LocalRT { get; set; }

        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "[Reunião Retrospectiva] Campo Data Inicial obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "[Reunião Retrospectiva] Campo Data Inicial inválido.")]
        public DateTime DataInicialRT { get; set; }

        [DisplayName("Data Final")]
        [Required(ErrorMessage = "[Reunião Retrospectiva] Campo Data Final obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "[Reunião Retrospectiva] Campo Data Final inválido.")]
        public DateTime DataFinalRT { get; set; }
        #endregion
        #endregion

        #endregion

        #region Methods
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SprintAtual.Id > 0 && DataInicial <= SprintAtual.DataFinal)
            {
                yield return
                    new ValidationResult(errorMessage: "[Sprint] Data Inicial deve ser maior do que a data final da sprint atual.",
                                         memberNames: new[] { "DataInicial" });
            }

            if (DataFinal <= DataInicial)
            {
                yield return
                    new ValidationResult(errorMessage: "[Sprint] Data Final deve ser maior do que a Data Inicial.",
                                         memberNames: new[] { "DataFinal" });
            }

            if (DataFinalRP <= DataInicialRP)
            {
                yield return
                    new ValidationResult(errorMessage: "[Reunião Planejamento] Data Final deve ser maior do que a Data Inicial.",
                                         memberNames: new[] { "DataFinalRP" });
            }

            if (DataInicialRP < DataInicial)
            {
                yield return
                    new ValidationResult(errorMessage: "[Reunião Planejamento] Data Inicial deve ser maior ou igual a data inicial da sprint.",
                                         memberNames: new[] { "DataInicialRP" });
            }

            if (DataFinalRD <= DataInicialRD)
            {
                yield return
                    new ValidationResult(errorMessage: "[Reunião Diária] Data Final deve ser maior do que a Data Inicial.",
                                         memberNames: new[] { "DataFinalRD" });
            }

            if (!WorkSun && !WorkMon && !WorkTue && !WorkWed && !WorkThu && !WorkFri && !WorkSat)
            {
                yield return
                    new ValidationResult(errorMessage: "[Reunião Diária] Campo Dias de Trabalho obrigatório.");
            }

            if (DataInicialRD <= DataFinalRP)
            {
                yield return
                    new ValidationResult(errorMessage: "[Reunião Diária] Data Inicial deve ser maior do que a data final da reunião de planejamento.",
                                         memberNames: new[] { "DataInicialRD" });
            }

            if (DataFinalRV <= DataInicialRV)
            {
                yield return
                    new ValidationResult(errorMessage: "[Reunião Revisão] Data Final deve ser maior do que a Data Inicial.",
                                         memberNames: new[] { "DataFinalRV" });
            }

            var time = HorarioInicialRD.Split(':');
            var DataFinalRD_Aux = DataFinalRD.Date.AddHours(Convert.ToInt16(time[0]))
                                                  .AddMinutes(Convert.ToInt16(time[1]) + 15);
            if (DataInicialRV <= DataFinalRD_Aux)
            {
                yield return
                    new ValidationResult(errorMessage: "[Reunião Revisão] Data Inicial deve ser maior do que a data final da reunião diária.",
                                         memberNames: new[] { "DataInicialRV" });
            }

            if (DataFinalRT <= DataInicialRT)
            {
                yield return
                    new ValidationResult(errorMessage: "[Reunião Retrospectiva] Data Final deve ser maior do que a Data Inicial.",
                                         memberNames: new[] { "DataFinalRT" });
            }

            if (DataInicialRT <= DataFinalRV)
            {
                yield return
                    new ValidationResult(errorMessage: "[Reunião Retrospectiva] Data Inicial deve ser maior do que a data final da reunião de revisão.",
                                         memberNames: new[] { "DataInicialRT" });
            }
        }
        #endregion
    }
}