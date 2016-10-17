using StartIdea.DataAccess;
using StartIdea.UI.Areas.Admin.Models;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.Admin.Controllers
{
    public class FuncaoController : AppController
    {
        private StartIdeaDBContext _dbContext;

        public FuncaoController(StartIdeaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}