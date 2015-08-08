using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cyclo.Models;

namespace Cyclo.Controllers
{
    public class JobsController : Controller
    {
        private CycloDBContext db = new CycloDBContext();

        // GET: Jobs
        public ActionResult Index()
        {
            return View(db.Jobs.ToList());
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int? id, int? eid)
        {
            if (id == null || eid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            ViewBag.linkedEvent = db.events.Include(e => e.subCategory).Include(e => e.subCategory.parent).Where(e => e.ID == eid).FirstOrDefault();
            if (job == null || ViewBag.linkedEvent == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create(int eid)
        {
            ViewBag.linkedEvent = db.events.Where(e => e.ID == eid).First();
            return View();
        }

        // POST: Jobs/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int eid, [Bind(Include = "ID,deadLine,name,description,report,userID")] Job job)
        {
            Event current = db.events.Include(e => e.Jobs).Where(e => e.ID == eid).First();
            ViewBag.linkedEvent = current;
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                current.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Details", "Events", new { id = eid });
            }

            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id, int? eid)
        {

            if (id == null || eid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            ViewBag.linkedEvent = db.events.Include(e => e.subCategory).Include(e => e.subCategory.parent).Where(e => e.ID == eid).FirstOrDefault();
            if (job == null || ViewBag.linkedEvent == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? eid, [Bind(Include = "ID,deadLine,name,description,report,userID")] Job job)
        {
            if (eid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.linkedEvent = db.events.Include(e => e.subCategory).Include(e => e.subCategory.parent).Where(e => e.ID == eid).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = job.ID, eid = eid });
            }
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id, int? eid)
        {
            if (id == null || eid==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Details", "Events", new { id=eid});
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
