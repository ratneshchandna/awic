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
    public class MemberController : Controller
    {
        private string UppercaseFirst(string s)
        {
            // Check for empty string
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            // Return char and concat substring of each word
            string[] words = s.Split(new char[] {' '});
            for(int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
            return String.Join(" ", words);
        }

        // GET: Member/Create
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Member member)
        {
            if (ModelState.IsValid)
            {
                member.Date = DateTime.Now.AddHours(3.0);

                int feeAmount;
                string feeOption;
                string paymentMethod;

                switch (member.FeeOption)
                {
                    case FeeOption.OneYearMembership: 
                        feeAmount = 10;
                        feeOption = "one year membership with us";
                        break;
                    case FeeOption.FiveYearMembership: 
                        feeAmount = 40;
                        feeOption = "five year membership with us";
                        break;
                    case FeeOption.LifetimeMembership: 
                        feeAmount = 200;
                        feeOption = "lifetime membership with us";
                        break;
                    default: 
                        feeAmount = 500;
                        feeOption = "one year patronage fee";
                        break;
                }

                switch (member.PaymentMethod)
                {
                    case PaymentMethod.Cash: 
                        paymentMethod = "cash";
                        break;
                    case PaymentMethod.Cheque: 
                        paymentMethod = "cheque";
                        break;
                    default: 
                        paymentMethod = "credit card";
                        break;
                }

                bool noExceptionThrown = false;

                try
                {
                    AWICEmailHelper.SendMemberRegistrationEmail(member, UppercaseFirst(feeOption.Replace(" with us", "")), UppercaseFirst(paymentMethod));

                    if(member.PaymentMethod == PaymentMethod.CreditCard)
                    {
                        ViewBag.FeeAmount = feeAmount;
                        ViewBag.FeeOption = feeOption;

                        return View("Payment", member);
                    }

                    noExceptionThrown = true;
                }
                catch(Exception e)
                {
                    // Error will be handled in code after if statement
                }

                if(noExceptionThrown)
                {
                    try
                    {
                        AWICEmailHelper.SendMemberRegistrationReceivedEmail(member, feeAmount, feeOption, paymentMethod);

                        ViewBag.FeeAmount = feeAmount;
                        ViewBag.FeeOption = feeOption;
                        ViewBag.PaymentMethod = paymentMethod;

                        return View("SignUpComplete", member);
                    }
                    catch(Exception e)
                    {
                        // Ignore, since email not sent to member is not an issue
                    }
                }
            }

            ModelState.AddModelError("", "Something went wrong while trying to submit your membership form. " +
                                         "Please fix any errors below, if any. Otherwise, please try again, or email " +
                                         "us your intent to sign up as a member of AWIC at " +
                                         AWIC.Models.User.ADMIN + ". ");

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(Member member, string stripeToken)
        {
            string token = stripeToken;
            
            int feeAmount;
            string feeOption;
            string paymentMethod;

            switch(member.FeeOption)
            {
                case FeeOption.OneYearMembership: 
                    feeAmount = 10;
                    feeOption = "one year membership with us";
                    break;
                case FeeOption.FiveYearMembership: 
                    feeAmount = 40;
                    feeOption = "five year membership with us";
                    break;
                case FeeOption.LifetimeMembership: 
                    feeAmount = 200;
                    feeOption = "lifetime membership with us";
                    break;
                default: 
                    feeAmount = 500;
                    feeOption = "one year patronage fee";
                    break;
            }

            switch (member.PaymentMethod)
            {
                case PaymentMethod.Cash: 
                    paymentMethod = "cash";
                    break;
                case PaymentMethod.Cheque: 
                    paymentMethod = "cheque";
                    break;
                default: 
                    paymentMethod = "credit card";
                    break;
            }

            var myCharge = new StripeChargeCreateOptions();
            myCharge.Amount = feeAmount * 100;
            myCharge.Currency = "cad";
            myCharge.Description = "Membership/Patronage Fee Payment";
            myCharge.Source = new StripeSourceOptions()
            {
                TokenId = token
            };

            var chargeService = new StripeChargeService();
            try
            {
                StripeCharge stripeCharge = chargeService.Create(myCharge);
            }
            catch (StripeException e)
            {
                if (e.StripeError.ErrorType == "card_error" || e.StripeError.ErrorType == "invalid_request_error")
                {
                    ViewBag.Error = e.StripeError.Message;
                }
                else
                {
                    ViewBag.Error = "Something went wrong on our side and we couldn't process your donation. We hope you don't mind trying to submit your donation again!";
                }

                ViewBag.FeeAmount = feeAmount;
                ViewBag.FeeOption = feeOption;

                return View(member);
            }

            try
            {
                AWICEmailHelper.SendFeePaidEmail(member, feeAmount, feeOption);
                AWICEmailHelper.SendMemberRegistrationReceivedEmail(member, feeAmount, feeOption, paymentMethod);
            }
            catch(Exception e)
            {
                // Ignore, since email not sent to member or AWIC is not an issue
            }

            ViewBag.FeeAmount = feeAmount;
            ViewBag.FeeOption = feeOption;
            ViewBag.PaymentMethod = paymentMethod;

            return View("SignUpComplete", member);
        }
    }
}