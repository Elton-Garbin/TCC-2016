using StartIdea.DataAccess;
using StartIdea.Model.ScrumEventos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StartIdea.UI.Controllers
{
    public class AgendaController : Controller
    {
        private StartIdeaDBContext _dbContext;

        public AgendaController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            int SprintId = GetSprintId();

            var query = _dbContext.Reunioes.Where(r => r.SprintId == SprintId
                                                    && DbFunctions.TruncateTime(r.DataInicial) == DateTime.Today)
                                           .OrderBy(r => r.DataInicial);

            List<string> compromissos = new List<string>();
            foreach (var row in query)
            {
                string tipoReuniao = "Diária";
                switch (row.TipoReuniao)
                {
                    case TipoReuniao.Planejamento:
                        tipoReuniao = "de Planejamento";
                        break;
                    case TipoReuniao.Revisao:
                        tipoReuniao = "de Revisão";
                        break;
                    case TipoReuniao.Retrospectiva:
                        tipoReuniao = "de Retrospectiva";
                        break;
                }

                compromissos.Add(string.Format("Reunião {0} hoje às {1}. Local: {2}", tipoReuniao, row.DataInicial, row.Local));
            }

            if (compromissos.Count == 0)
                compromissos.Add("Nehuma reunião agendada para o dia de hoje.");

            return Json(compromissos, JsonRequestBehavior.AllowGet);
        }

        private int GetSprintId()
        {
            var sprint = _dbContext.Sprints.FirstOrDefault(s => !s.DataCancelamento.HasValue
                                                              && s.TimeId == 1
                                                              && s.DataInicial <= DateTime.Now
                                                              && s.DataFinal >= DateTime.Now) ?? new Sprint();

            return sprint.Id;
        }
    }
}