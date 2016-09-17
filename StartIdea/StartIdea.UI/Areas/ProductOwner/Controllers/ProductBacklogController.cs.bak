using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ProductOwner.Controllers
{
    public class ProductBacklogController : Controller
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public ActionResult Index()
        {
            return View(dbContext.ProductBacklogs.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserStory,Prioridade")] ProductBacklog productBacklog)
        {
            if (ModelState.IsValid)
            {
                productBacklog.ProductOwnerId = 1; // Remover

                dbContext.ProductBacklogs.Add(productBacklog);
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(productBacklog);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBacklog productBacklog = dbContext.ProductBacklogs.Find(id);
            if (productBacklog == null)
                return HttpNotFound();

            return View(productBacklog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserStory,StoryPoint,Prioridade,DataInclusao,ProductOwnerId")] ProductBacklog productBacklog)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(productBacklog).State = EntityState.Modified;
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(productBacklog);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBacklog productBacklog = dbContext.ProductBacklogs.Find(id);
            if (productBacklog == null)
                return HttpNotFound();

            dbContext.ProductBacklogs.Remove(productBacklog);
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