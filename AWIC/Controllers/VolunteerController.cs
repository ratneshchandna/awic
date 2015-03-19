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
using AWIC.Helpers;

namespace AWIC.Controllers
{
    public class VolunteerController : Controller
    {
        // GET: Volunteer/Create
        public ActionResult Apply()
        {
            return View();
        }

        // POST: Volunteer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Apply(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                if(SendEmails(volunteer))
                {
                    ViewBag.Name = volunteer.Name;
                    ViewBag.EmailAddress = volunteer.EmailAddress;
                    ViewBag.Phone = volunteer.Phone;

                    return View("ApplicationReceived");
                }
            }

            ModelState.AddModelError("", "Something went wrong while trying to submit your volunteer application. " + 
                                         "Please fix any errors below, if any. Otherwise, please try again, or email " + 
                                         "us your intent to apply to become a volunteer for AWIC at " + 
                                         AWIC.Models.User.ADMIN + ". ");

            return View(volunteer);
        }

        private bool SendEmails(Volunteer volunteer)
        {
            try
            {
                AWICEmailHelper.SendVolunteerApplicationEmail(volunteer);
            }
            catch (Exception e)
            {
                return false;
            }

            try
            {
                AWICEmailHelper.SendVolunteerApplicationReceivedEmail(volunteer);
            }
            catch (Exception e)
            {
                // No need to do anything here, since failure to send an email to the 
                // volunteer does not come in the way of his/her application processs
            }

            return true;
        }
    }
}