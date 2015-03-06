using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AWIC.DAL;
using AWIC.Models;
using System.Data.Entity.Infrastructure;

namespace AWIC.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private AWICDbContext db = new AWICDbContext();

        // GET: Event
        public async Task<ActionResult> Index()
        {
            return View(await db.Events.OrderBy(e => e.EventDateAndTime).ToListAsync());
        }

        // GET: Event/Details/5
        /*public async Task<ActionResult> Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }*/

        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AllDay,EventDateAndTime,EventDescription")] Event @event, string weekly)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);

                if (weekly != null)
                {
                    DateTime eventDateAndTime = @event.EventDateAndTime.AddDays(-7);
                    int eventMonth = @event.EventDateAndTime.Month;

                    while(eventDateAndTime.Month == eventMonth)
                    {
                        db.Events.Add(new Event
                        {
                            AllDay = @event.AllDay,
                            EventDateAndTime = eventDateAndTime,
                            EventDescription = @event.EventDescription
                        });

                        eventDateAndTime = eventDateAndTime.AddDays(-7);
                    }

                    eventDateAndTime = @event.EventDateAndTime.AddDays(7);

                    while (eventDateAndTime.Month == eventMonth)
                    {
                        db.Events.Add(new Event
                        {
                            AllDay = @event.AllDay,
                            EventDateAndTime = eventDateAndTime,
                            EventDescription = @event.EventDescription
                        });

                        eventDateAndTime = eventDateAndTime.AddDays(7);
                    }
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Event/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int id, string weekly)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var eventToUpdate = await db.Events.FindAsync(id);
            if (TryUpdateModel(eventToUpdate, "", new string[] { "AllDay", "EventDateAndTime", "EventDescription" }))
            {
                try
                {
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */ )
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(eventToUpdate);
        }

        // GET: Event/Delete/5
        /*public async Task<ActionResult> Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }*/

        // GET: Event/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            db.Events.Remove(@event);
            await db.SaveChangesAsync();
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
