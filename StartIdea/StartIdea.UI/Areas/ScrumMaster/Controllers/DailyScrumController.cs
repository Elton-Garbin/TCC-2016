using StartIdea.DataAccess;
using StartIdea.UI.Areas.ScrumMaster.Models;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ScrumMaster.Controllers
{
    public class DailyScrumController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public DailyScrumController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}