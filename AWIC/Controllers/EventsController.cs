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
using AWIC.Helpers;
using System.Linq;

namespace AWIC.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private AWICDbContext db = new AWICDbContext();
        private string[] Months = new string[] { 
                "January", "February", "March", "April", "May", "June", "July", 
                "August", "September", "October", "November", "December" };
        DateTime today = DateTime.Now.AddHours(3.0);

        // GET: Event
        public async Task<ActionResult> Index(string sortOrder)
        {
            ViewBag.EventDateAndTimeSortParm = String.IsNullOrEmpty(sortOrder) ? "event_date_and_time_desc" : "";
            ViewBag.EventDescriptionSortParm = sortOrder == "event_description" ? "event_description_desc" : "event_description";
            ViewBag.AllDayOrTBDSortParam = sortOrder == "all_day" ? "all_day_desc" : "all_day";

            string beginningOfMonth = Months[((today.Month) - 1)] + " 01, " + today.Year;
            DateTime beginningOfMonthDate = DateTime.Parse(beginningOfMonth);
            var eventsToDelete = db.Events.Where(e => e.EventDateAndTime < beginningOfMonthDate);
            if(eventsToDelete.Count() > 0)
            {
                foreach(Event @event in eventsToDelete)
                {
                    db.Events.Remove(@event);
                }
                await db.SaveChangesAsync();
            }

            var events = from s in db.Events
                         select s;

            switch(sortOrder)
            {
                case "event_date_and_time_desc":
                    events = events.OrderByDescending(e => e.EventDateAndTime);
                    break;
                case "event_description":
                    events = events.OrderBy(e => e.EventDescription);
                    break;
                case "event_description_desc":
                    events = events.OrderByDescending(e => e.EventDescription);
                    break;
                case "all_day":
                    events = events.OrderBy(e => e.AllDayOrTBD);
                    break;
                case "all_day_desc":
                    events = events.OrderByDescending(e => e.AllDayOrTBD);
                    break;
                default:
                    events = events.OrderBy(e => e.EventDateAndTime);
                    break;
            }

            return View(await events.ToListAsync());
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

        private async Task AddWeeklyEventsAsync(Event @event, bool ignoreReferenceEvent)
        {
            if(!ignoreReferenceEvent)
            {
                db.Events.Add(@event);
            }

            // Keep track of repeated event's all day or TBD flag and description, 
            // as well as a list of dates it repeats on. Once this information 
            // is collected (after the 2 while loops), it will be used to search 
            // for the repeated events in the DB (after they've been committed) 
            // and the their IDs will be written to the WeeklyDates property in 
            // each of those events

            bool repeatedAllDayOrTBD = @event.AllDayOrTBD;
            string repeatedEventDescription = @event.EventDescription;
            List<DateTime> repeatedDatesAndTimes = new List<DateTime>();
            repeatedDatesAndTimes.Add(@event.EventDateAndTime);

            int eventMonth = @event.EventDateAndTime.Month;
            DateTime eventDateAndTime = @event.EventDateAndTime.AddDays(-7);

            while (eventDateAndTime.Month == eventMonth)
            {
                db.Events.Add(new Event
                {
                    AllDayOrTBD = @event.AllDayOrTBD,
                    EventDateAndTime = eventDateAndTime,
                    EventDescription = @event.EventDescription
                });

                repeatedDatesAndTimes.Add(eventDateAndTime);

                eventDateAndTime = eventDateAndTime.AddDays(-7);
            }

            eventDateAndTime = @event.EventDateAndTime.AddDays(7);

            while (eventDateAndTime.Month == eventMonth)
            {
                db.Events.Add(new Event
                {
                    AllDayOrTBD = @event.AllDayOrTBD,
                    EventDateAndTime = eventDateAndTime,
                    EventDescription = @event.EventDescription
                });

                repeatedDatesAndTimes.Add(eventDateAndTime);

                eventDateAndTime = eventDateAndTime.AddDays(7);
            }

            await db.SaveChangesAsync();

            string IDs = "";
            foreach (DateTime dateAndTime in repeatedDatesAndTimes)
            {
                Event repeatedEvent = db.Events.FirstOrDefault(
                                                                e => e.AllDayOrTBD == repeatedAllDayOrTBD &&
                                                                e.EventDescription == repeatedEventDescription &&
                                                                e.EventDateAndTime == dateAndTime
                                                              );
                if (repeatedEvent != null)
                {
                    if (String.IsNullOrEmpty(IDs))
                    {
                        IDs = repeatedEvent.ID.ToString();
                    }
                    else
                    {
                        IDs = IDs + "," + repeatedEvent.ID.ToString();
                    }
                }
            }
            foreach (DateTime dateAndTime in repeatedDatesAndTimes)
            {
                Event repeatedEvent = db.Events.FirstOrDefault(
                                                                e => e.AllDayOrTBD == repeatedAllDayOrTBD &&
                                                                e.EventDescription == repeatedEventDescription &&
                                                                e.EventDateAndTime == dateAndTime
                                                              );
                if (repeatedEvent != null)
                {
                    repeatedEvent.WeeklyDates = IDs;
                }
                db.Entry(repeatedEvent).State = EntityState.Modified;
            }

            await db.SaveChangesAsync();
        }

        // POST: Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AllDayOrTBD,EventDateAndTime,EventDescription")] Event @event, string weekly)
        {
            if (ModelState.IsValid)
            {
                string beginningOfMonth = Months[((today.Month) - 1)] + " 01, " + today.Year;
                DateTime beginningOfMonthDate = DateTime.Parse(beginningOfMonth);
                if(@event.EventDateAndTime < beginningOfMonthDate)
                {
                    ModelState.AddModelError("EventDateAndTime", "The event's date and time cannot be before " + beginningOfMonth);

                    return View(@event);
                }

                if (weekly != null)
                {
                    await AddWeeklyEventsAsync(@event, false);
                }
                else
                {
                    db.Events.Add(@event);

                    await db.SaveChangesAsync();
                }

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
        public async Task<ActionResult> EditPost(int id, string makeweekly, string makenonweekly)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event eventToUpdate = await db.Events.FindAsync(id);

            if (eventToUpdate == null)
            {
                return HttpNotFound();
            }

            DateTime editedDateAndTime;
            if(!DateTime.TryParse(Request.Form.Get("EventDateAndTime"), out editedDateAndTime))
            {
                ModelState.AddModelError("EventDateAndTime", "The date and time entered is not valid. Please change it to a valid date. ");

                return View(eventToUpdate);
            }

            Event editedEvent = new Event
            {
                AllDayOrTBD = Boolean.Parse(Request.Form.Get("AllDayOrTBD").Split(',')[0]), 
                EventDateAndTime = editedDateAndTime, 
                EventDescription = Request.Form.Get("EventDescription")
            };

            string beginningOfMonth = Months[((today.Month) - 1)] + " 01, " + today.Year;
            DateTime beginningOfMonthDate = DateTime.Parse(beginningOfMonth);

            if (editedEvent.EventDateAndTime < beginningOfMonthDate)
            {
                ModelState.AddModelError("EventDateAndTime", "The event's date and time cannot be before " + beginningOfMonth);

                return View(eventToUpdate);
            }

            if(!String.IsNullOrEmpty(eventToUpdate.WeeklyDates))
            {
                if(String.IsNullOrEmpty(makenonweekly))
                {
                    if (editedEvent.EventDateAndTime == eventToUpdate.EventDateAndTime)
                    {
                        string[] weeklyDates = eventToUpdate.WeeklyDates.Split(',');

                        foreach (string weeklyDate in weeklyDates)
                        {
                            int ID = int.Parse(weeklyDate);

                            if (ID != null)
                            {
                                Event repeatedEventToUpdate = await db.Events.FindAsync(ID);

                                if (repeatedEventToUpdate != null)
                                {
                                    if (!TryUpdateModel(repeatedEventToUpdate, "", new string[] { "AllDayOrTBD", "EventDescription" }))
                                    {
                                        return View(eventToUpdate);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Delete weekly events
                        await DeleteWeeklyEventsAsync(eventToUpdate, false);

                        // Add weekly events
                        await AddWeeklyEventsAsync(editedEvent, false);
                    }

                    try
                    {
                        await db.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    catch (Exception /* dex */ )
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log. 
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }
                }
                else
                {
                    if (TryUpdateModel(eventToUpdate, "", new string[] { "AllDayOrTBD", "EventDateAndTime", "EventDescription" }))
                    {
                        try
                        {
                            await DeleteWeeklyEventsAsync(eventToUpdate, true);

                            eventToUpdate.WeeklyDates = null;
                            db.Entry(eventToUpdate).State = EntityState.Modified;

                            await db.SaveChangesAsync();

                            return RedirectToAction("Index");
                        }
                        catch (Exception /* dex */ )
                        {
                            //Log the error (uncomment dex variable name and add a line here to write a log. 
                            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        }
                    }
                }
            }
            else
            {
                if(String.IsNullOrEmpty(makeweekly))
                {
                    if (TryUpdateModel(eventToUpdate, "", new string[] { "AllDayOrTBD", "EventDateAndTime", "EventDescription" }))
                    {
                        try
                        {
                            await db.SaveChangesAsync();

                            return RedirectToAction("Index");
                        }
                        catch (Exception /* dex */ )
                        {
                            //Log the error (uncomment dex variable name and add a line here to write a log. 
                            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        }
                    }
                }
                else
                {
                    if (TryUpdateModel(eventToUpdate, "", new string[] { "AllDayOrTBD", "EventDateAndTime", "EventDescription" }))
                    {
                        try
                        {
                            await db.SaveChangesAsync();

                            await AddWeeklyEventsAsync(eventToUpdate, true);

                            return RedirectToAction("Index");
                        }
                        catch (Exception /* dex */ )
                        {
                            //Log the error (uncomment dex variable name and add a line here to write a log. 
                            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        }
                    }
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

        private async Task DeleteWeeklyEventsAsync(Event @event, bool ignoreReferenceEvent)
        {
            string[] weeklyDatesToDelete = @event.WeeklyDates.Split(',');

            foreach(string weeklyDateToDelete in weeklyDatesToDelete)
            {
                int ID;
                if(!int.TryParse(weeklyDateToDelete, out ID))
                {
                    ID = -1;
                }

                if(ID != -1)
                {
                    if(ignoreReferenceEvent && ID == @event.ID)
                    {
                        continue;
                    }
                    Event repeatedEventToDelete = await db.Events.FindAsync(ID);

                    if (repeatedEventToDelete != null)
                    {
                        db.Events.Remove(repeatedEventToDelete);
                    }
                }
            }

            await db.SaveChangesAsync();
        }

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
            
            if(String.IsNullOrEmpty(@event.WeeklyDates))
            {
                db.Events.Remove(@event);
            }
            else
            {
                await DeleteWeeklyEventsAsync(@event, false);
            }

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<string> GetDaysWithEvents(CalendarData calendarData)
        {
            string daysWithEvents = "";

            if(Request.IsAjaxRequest())
            {
                DateTime date = DateTime.Parse(calendarData.currentYear + "/" + calendarData.currentMonth + "/1");
                int month = date.Month;
                List<Event> allEvents = await db.Events.ToListAsync();

                while(date.Month == month)
                {
                    if(allEvents.Find(e => e.EventDateAndTime.ToShortDateString() == date.ToShortDateString()) != null)
                    {
                        if(!String.IsNullOrEmpty(daysWithEvents))
                        {
                            daysWithEvents = daysWithEvents + ",";
                        }

                        daysWithEvents = daysWithEvents + date.Day.ToString();
                    }

                    date = date.AddDays(1);
                }
            }

            return daysWithEvents;
        }

        [AllowAnonymous]
        public async Task<PartialViewResult> GetEventsForDay(CalendarData calendarData)
        {
            IEnumerable<Event> eventsForDay = new List<Event>();

            if(Request.IsAjaxRequest())
            {
                eventsForDay = db.Events.Where(e =>
                    e.EventDateAndTime.Day.ToString() == calendarData.currentDay &&
                    e.EventDateAndTime.Month.ToString() == calendarData.currentMonth &&
                    e.EventDateAndTime.Year.ToString() == calendarData.currentYear
                    ).OrderBy(e => e.EventDateAndTime);
            }

            return PartialView("_EventsForDay", eventsForDay);
        }

        [AllowAnonymous]
        public async Task<PartialViewResult> GetEventsForWeek(CalendarData calendarData)
        {
            List<IGrouping<int, AWIC.Models.Event>> daysOfEvents = new List<IGrouping<int, AWIC.Models.Event>>();

            if (Request.IsAjaxRequest())
            {
                List<string> daysOfWeek = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                DateTime day = DateTime.Parse(calendarData.currentYear + "/" + calendarData.currentMonth + "/" + calendarData.currentDay);
                int dayNumber = day.Day;
                int monthNumber = day.Month;
                int yearNumber = day.Year;
                int dayNumberOfWeek = daysOfWeek.IndexOf(day.DayOfWeek.ToString()) + 1;
                int startDay;
                int endDay;

                if (dayNumber < dayNumberOfWeek)
                {
                    startDay = 1;
                    endDay = dayNumber + (7 - dayNumberOfWeek);
                }
                else if((dayNumber + (7 - dayNumberOfWeek)) > (DateTime.DaysInMonth(yearNumber,monthNumber)))
                {
                    startDay = dayNumber - (dayNumberOfWeek - 1);
                    endDay = DateTime.DaysInMonth(yearNumber, monthNumber);
                }
                else
                {
                    startDay = dayNumber - (dayNumberOfWeek - 1);
                    endDay = dayNumber + (7 - dayNumberOfWeek);
                }

                daysOfEvents = await db.Events.Where(e =>
                    (e.EventDateAndTime.Day >= startDay && e.EventDateAndTime.Day <= endDay) && 
                    e.EventDateAndTime.Month == monthNumber &&
                    e.EventDateAndTime.Year == yearNumber
                ).OrderBy(e => e.EventDateAndTime).GroupBy(e => e.EventDateAndTime.Day).ToListAsync();
            }

            return PartialView("_EventsForMonth", daysOfEvents);
        }

        [AllowAnonymous]
        public async Task<PartialViewResult> GetEventsForMonth(CalendarData calendarData)
        {
            List<IGrouping<int, AWIC.Models.Event>> daysOfEvents = new List<IGrouping<int, AWIC.Models.Event>>();

            if (Request.IsAjaxRequest())
            {
                daysOfEvents = await db.Events.Where(e =>
                    e.EventDateAndTime.Month.ToString() == calendarData.currentMonth &&
                    e.EventDateAndTime.Year.ToString() == calendarData.currentYear
                    ).OrderBy(e => e.EventDateAndTime).GroupBy(e => e.EventDateAndTime.Day).ToListAsync();
            }

            return PartialView("_EventsForMonth", daysOfEvents);
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
