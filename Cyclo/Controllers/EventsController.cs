using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cyclo.Models;
using System.Globalization;
using Microsoft.AspNet.Identity;

namespace Cyclo.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private CycloDBContext db = new CycloDBContext();
        private ApplicationDbContext userdb = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index(int? m, int? y)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            ViewBag.rights = userdb.Users.Find(User.Identity.GetUserId()).Rights;
            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);


            if (m != null && y!=null) currentDate = new DateTime(y.Value,m.Value,1);
            List<Event> events = db.events.Include(e=>e.subCategory).Where(h => ((h.startDate.Month == currentDate.Month || h.endDate.Month == currentDate.Month) || (currentDate.Month > h.startDate.Month && currentDate.Month < h.endDate.Month))).ToList();
            ViewBag.SubCategoryList = db.subCategories.Include(c=>c.parent).OrderBy(c=>c.parent.ID).ToList();
            ViewBag.weekNums = cal.GetWeekOfYear(currentDate.AddMonths(1).AddDays(-1), dfi.CalendarWeekRule, dfi.FirstDayOfWeek) - cal.GetWeekOfYear(currentDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek)+1;
            ViewBag.currentDate = currentDate;

            return View(events);
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id, int? m, int? y)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.categories.Load();
            ViewBag.users = userdb.Users.ToList();
            Event @event = db.events.Include(e => e.subCategory).Include(e=>e.Jobs).Where(e => e.ID == id).First();
            ViewBag.month = @event.startDate.Month;
            ViewBag.year = @event.startDate.Year;
            var rights = userdb.Users.Find(User.Identity.GetUserId()).Rights;
            if (rights.Contains(@event.subCategory.ID))
            {
                ViewBag.edit = true;
            }
            else
            {
                ViewBag.edit = false;
            }
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create(int cat, int m, int y, int w)
        {
            ViewBag.subCategory = db.subCategories.Include(c => c.parent).Where(c => c.ID == cat).First();
            ViewBag.month = m;
            ViewBag.year = y;
            ViewBag.week = w;

            DateTime date = new DateTime(y, m, 1);
            DateTime start, end;
            if (w==1)
            {
                start = date;
                end = date.AddDays(7 - (int)date.DayOfWeek);
            }
            else
            {
                start = date.AddDays(1+(7 - (int)date.DayOfWeek)+(w-2)*7);
                end = date.AddDays(7 - (int)date.DayOfWeek + (w-1) * 7);
                while (end.Month != start.Month) end=end.AddDays(-1);
            }
            Event ev = new Event();
            ev.startDate = start;
            ev.endDate = end;
            return View(ev);
        }

        // POST: Events/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int cat, int m, int y, int w, Event @event)
        {
            Event ev = new Event();
            ev.name = @event.name;
            ev.description = @event.description;
            ev.startDate = @event.startDate;
            ev.endDate = @event.endDate;
            ev.subCategory = db.subCategories.Where(s => s.ID == cat).First();
            if (ModelState.IsValid)
            {
                db.events.Add(ev);
                db.SaveChanges();
                return RedirectToAction("Details", new {id=ev.ID, m=m, y=y });
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.categories.Load();
            Event @event = db.events.Include(e => e.subCategory).Where(e=>e.ID==id).First();
            ViewBag.month = @event.startDate.Month;
            ViewBag.year = @event.startDate.Year;
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,startDate,endDate,description,name")] Event @event)
        {
            ViewBag.month = @event.startDate.Month;
            ViewBag.year = @event.startDate.Year;
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { id=@event.ID});
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id, int m, int y)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.events.Find(id);

            db.events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index", new {m=m,y=y});
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.events.Find(id);
            db.events.Remove(@event);
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
