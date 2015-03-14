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
using Stripe;

namespace AWIC.Controllers
{
    public class DonationsController : Controller
    {
        private AWICDbContext db = new AWICDbContext();

        // GET: Donations
        public async Task<PartialViewResult> GetLatestDonations()
        {
            return PartialView("_LatestDonations", await db.Donations.ToListAsync());
        }

        // GET: Donations/Donate
        public ActionResult Donate()
        {
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Donate([Bind(Include = "Donor,DonorEmail,AmountInCAD")] Donations donation)
        {
            if (ModelState.IsValid)
            {
                return View("Payment", donation);
            }

            return View(donation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Payment(Donations donationToProcess, string stripeToken)
        {
            if(ModelState.IsValid)
            {
                string token = stripeToken; //Request.Form.GetValues("stripeToken").GetValue(0);

                var myCharge = new StripeChargeCreateOptions();
                myCharge.Amount = (int)(donationToProcess.AmountInCAD * 100);
                myCharge.Currency = "cad";
                myCharge.Description = "Donation to AWIC";
                myCharge.Source = new StripeSourceOptions()
                {
                    TokenId = token
                };

                var chargeService = new StripeChargeService();
                try
                {
                    StripeCharge stripeCharge = chargeService.Create(myCharge);
                }
                catch(StripeException e)
                {
                    if (e.StripeError.ErrorType == "card_error" || e.StripeError.ErrorType == "invalid_request_error")
                    {
                        ViewBag.Error = e.StripeError.Message;
                    }
                    else
                    {
                        ViewBag.Error = "Something went wrong on our side and we couldn't process your donation. We hope you don't mind trying to submit your donation again!";
                    }
                    
                    return View(donationToProcess);
                }

                try
                {
                    donationToProcess.DonationDateAndTime = DateTime.Now;

                    db.Donations.Add(donationToProcess);

                    await db.SaveChangesAsync();

                    if (db.Donations.Count() > 5)
                    {
                        DateTime earliestDonationDateAndTime = await db.Donations.MinAsync(d => d.DonationDateAndTime);
                        Donations earliestDonation = await db.Donations.FirstOrDefaultAsync(d => d.DonationDateAndTime == earliestDonationDateAndTime);

                        db.Donations.Remove(earliestDonation);

                        await db.SaveChangesAsync();
                    }
                }
                catch(Exception e)
                {
                    // We weren't able to make database changes, which aren't all that crucial, 
                    // as long as the actual donation was successful
                }

                if (SendEmails(donationToProcess))
                {
                    ViewBag.SentEmailToDonor = true;
                }
                else
                {
                    ViewBag.SentEmailToDonor = false;
                }

                return View("DonationReceived", donationToProcess);
            }

            TempData["Error"] = "Something went wrong on our side and we couldn't process your donation. We hope you don't mind trying to submit your donation again!";
            return RedirectToAction("Donate");
        }

        private bool SendEmails(Donations processedDonation)
        {
            AWICEmailHelper emailHelper = new AWICEmailHelper();
            bool SentEmailToDonor = false;

            if (!String.IsNullOrEmpty(processedDonation.DonorEmail))
            {
                try
                {
                    emailHelper.SendDonationReceiptEmail(processedDonation.DonorEmail, processedDonation);
                    SentEmailToDonor = true;
                }
                catch (Exception e)
                {
                    // We weren't able to send an email to the donor, which isn't all that crucial
                    // as long as the actual donation was successful (and we return false from this 
                    // method
                }
            }

            try
            {
                emailHelper.SendDonationReceivedEmail(AWIC.Models.User.ADMIN, processedDonation);
            }
            catch(Exception e)
            {
                // We weren't able to send an email to AWIC, which isn't all that crucial
                // as long as the actual donation was successful
            }

            return SentEmailToDonor;
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
