using System.Web.Mvc;
using StartIdea.UI.Areas.TeamMember.Models;
using StartIdea.DataAccess;

namespace StartIdea.UI.Areas.TeamMember.Controllers
{
    public class StatusTarefaController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public StatusTarefaController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}