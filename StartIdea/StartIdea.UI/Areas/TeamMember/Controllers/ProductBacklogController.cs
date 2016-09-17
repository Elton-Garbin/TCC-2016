using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.TeamMember.Controllers
{
    public class ProductBacklogController : Controller
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public ActionResult Index()
        {
            var productBacklogs = dbContext.ProductBacklogs;
            return View(productBacklogs.ToList());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var productBacklog = dbContext.ProductBacklogs.Find(id);
            if (productBacklog == null)
                return HttpNotFound();

            return View(productBacklog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StoryPoint")] ProductBacklog productBacklog)
        {
            var productBacklogAux = dbContext.ProductBacklogs.Find(productBacklog.Id);
            productBacklogAux.StoryPoint = productBacklog.StoryPoint;

            dbContext.Entry(productBacklogAux).State = EntityState.Modified;
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}
