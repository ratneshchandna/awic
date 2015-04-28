using System;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using AWIC.Models;

namespace AWIC.Helpers
{
    public class SMTPServer
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class EmailConfig
    {
        public string From { get; set; }
        public SMTPServer SMTPServer { get; set; }

    }

    public class EmailHelper
    {
        private static EmailConfig _config { get; set; }

        private static void SetupConfig()
        {
            _config = new EmailConfig
                {
                    From = User.ADMIN,
                    SMTPServer = new SMTPServer
                    {
                        Address = System.Configuration.ConfigurationManager.AppSettings["SMTPServerAddress"],
                        Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SMTPServerPort"]),
                        Username = User.ADMIN,
                        Password = System.Configuration.ConfigurationManager.AppSettings["SMTPServerPassword"]
                    }
                };
        }

        protected static void SendEmail(string To, string Subject, string HtmlBody)
        {
            SetupConfig();

            MailMessage msg = new MailMessage(_config.From, To);
            msg.Subject = Subject;
            msg.Body = HtmlBody;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(_config.SMTPServer.Address, _config.SMTPServer.Port);
            client.Credentials = new NetworkCredential(_config.SMTPServer.Username, _config.SMTPServer.Password);
            client.Send(msg);
        }
    }

    public class AWICEmailHelper : EmailHelper
    {
        public static void SendVolunteerApplicationEmail(Volunteer volunteer)
        {
            string Subject = "New Volunteer Application - " + volunteer.Name;

            string HTMLBody = 
                "<p>An application was submitted by " + volunteer.Name + " to become a volunteer at AWIC. </p>" + 
                "<br />" + 
                "<p>Here is the application: </p>" +
                "<p>Name: " + volunteer.Name + "</p>" + 
                "<p>Address: " + volunteer.Address + "</p>" + 
                "<p>City: " + volunteer.City + "</p>" + 
                "<p>Province or State: " + volunteer.ProvinceOrState + "</p>" + 
                "<p>Country: " + volunteer.Country + "</p>" + 
                "<p>Postal Code: " + volunteer.PostalCode + "</p>" + 
                "<p>Phone Number: " + volunteer.Phone + "</p>" + 
                "<p>E-mail Address: " + volunteer.EmailAddress + "</p>" + 
                ( volunteer.DateOfBirth != null ? "<p>Date of Birth: " + ((DateTime)volunteer.DateOfBirth).ToLongDateString() + "</p>" : "" ) + 
                ( !String.IsNullOrEmpty(volunteer.LanguagesSpokenFluently) ? "<p>Languages Spoken Fluently: " + volunteer.LanguagesSpokenFluently + "</p>" : "" ) + 
                "<p>Qualifications: </p>" + 
                "<p>" + volunteer.Qualifications + "</p>" +
                "<p>Days and Times Available: </p>" + 
                ( volunteer.Monday    ? "<p>Monday"    + ( !String.IsNullOrEmpty(volunteer.MondayTimes)    ? ": " + volunteer.MondayTimes    : "" ) + "</p>" : "" ) +
                ( volunteer.Tuesday   ? "<p>Tuesday"   + ( !String.IsNullOrEmpty(volunteer.TuesdayTimes)   ? ": " + volunteer.TuesdayTimes   : "" ) + "</p>" : "" ) +
                ( volunteer.Wednesday ? "<p>Wednesday" + ( !String.IsNullOrEmpty(volunteer.WednesdayTimes) ? ": " + volunteer.WednesdayTimes : "" ) + "</p>" : "" ) +
                ( volunteer.Thursday  ? "<p>Thursday"  + ( !String.IsNullOrEmpty(volunteer.ThursdayTimes)  ? ": " + volunteer.ThursdayTimes  : "" ) + "</p>" : "" ) +
                ( volunteer.Friday    ? "<p>Friday"    + ( !String.IsNullOrEmpty(volunteer.FridayTimes)    ? ": " + volunteer.FridayTimes    : "" ) + "</p>" : "" ) +
                ( volunteer.Saturday  ? "<p>Saturday"  + ( !String.IsNullOrEmpty(volunteer.SaturdayTimes)  ? ": " + volunteer.SaturdayTimes  : "" ) + "</p>" : "" ) +
                ( volunteer.Sunday    ? "<p>Sunday"    + ( !String.IsNullOrEmpty(volunteer.SundayTimes)    ? ": " + volunteer.SundayTimes    : "" ) + "</p>" : "" );

            SendEmail(User.ADMIN, Subject, HTMLBody);
        }

        public static void SendVolunteerApplicationReceivedEmail(Volunteer volunteer)
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string path = urlHelper.Action("Contact", "Home");
            Uri fullURL = new Uri(HttpContext.Current.Request.Url, path);

            string Subject = "Your Volunteer Application for AWIC";

            string HTMLBody = 
                "<p>Hi " + volunteer.Name + ", </p>" +
                "<p>Thank you for your interest in volunteering with us! </p>" +
                "<p>We have received your application and will begin reviewing it soon. Once we've reviewed your " +
                    "application, we'll let you know via email (at " + volunteer.EmailAddress + "), or phone (at " +
                    volunteer.Phone + ") about whether or not we want you to come in for an interview. </p>" +
                "<p>If you have any questions or concerns about anything until then, feel free to reply back with your " +
                    "message and we'll be glad to address it. You can also call Elmira Mammadova at (416) 499-4144 with " + 
                    "your message, who will be more than glad to help you. </p>" +
                "<br />" +
                "<p>Sincerely, </p>" +
                "<p>The AWIC Team</p>" +
                "<br />" +
                "<p>PS You can find our general contact information <a href=\"" + fullURL + "\">here</a>. </p>";

            SendEmail(volunteer.EmailAddress, Subject, HTMLBody);
        }

        public static void SendMemberRegistrationEmail(Member member, string feeOption, string paymentMethod)
        {
            string Subject = 
                (
                    member.MembershipType == MembershipType.New ? 
                        (
                            member.FeeOption != FeeOption.OneYearPatronage ? 
                                "New Member Registration - " : 
                                "New Patron Registration - "
                        ) : 
                        (
                            member.FeeOption != FeeOption.OneYearPatronage ?
                                "Membership Renewal - " : 
                                "Patronage Renewal - "
                        )
                ) + 
                member.FirstName + " " + member.LastName;

            string HTMLBody =
                "<p>A " + 
                    (
                        member.MembershipType == MembershipType.New ? 
                            (
                                member.FeeOption != FeeOption.OneYearPatronage ? 
                                    "membership" :
                                    "patron-ship"
                            ) : 
                            (
                                member.FeeOption != FeeOption.OneYearPatronage ?
                                    "membership renewal" :
                                    "patron-ship renewal"
                            )
                    ) + 
                    " form was submitted by " + member.FirstName + " " + member.LastName + "</p>" + 
                "<br />" +
                "<p>Here is the form: </p>" +
                "<p>Date: " + member.Date.ToLongDateString() + "</p>" +
                "<p>Membership Type: " + member.MembershipType.ToString() + "</p>" +
                "<p>First Name: " + member.FirstName + "</p>" +
                "<p>Last Name: " + member.LastName + "</p>" +
                "<p>Address: " + member.Address + "</p>" +
                "<p>City: " + member.City + "</p>" +
                "<p>Province or State: " + member.ProvinceOrState + "</p>" +
                "<p>Country: " + member.Country + "</p>" +
                "<p>Postal Code: " + member.PostalCode + "</p>" +
                "<p>Phone Number: " + member.Phone + "</p>" +
                "<p>E-mail Address: " + member.EmailAddress + "</p>" +
                (!String.IsNullOrEmpty(member.ReferredBy) ? "<p>Referred By: " + member.ReferredBy + "</p>" : "") +
                "<p>Fee Option: " + feeOption + "</p>" +
                "<p>Payment Method: " + paymentMethod + "</p>";

            SendEmail(User.ADMIN, Subject, HTMLBody);
        }

        public static void SendMemberRegistrationReceivedEmail(Member member, int amount, string feeOption, string paymentMethod)
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string path = urlHelper.Action("Contact", "Home");
            Uri fullURL = new Uri(HttpContext.Current.Request.Url, path);

            string Subject = 
                "Your " + 
                (
                    member.MembershipType == MembershipType.New ?
                        (
                            member.FeeOption != FeeOption.OneYearPatronage ?
                                "membership at " :
                                "patronage at "
                        ) :
                        (
                            member.FeeOption != FeeOption.OneYearPatronage ?
                                "membership renewal for " :
                                "patronage renewal for "
                        )
                ) + 
                "AWIC";
            // : 
            string HTMLBody =
                "<p>Hi " + member.FirstName + ", </p>" + 
                (
                    member.PaymentMethod == PaymentMethod.CreditCard ? 
                    (   
                        (
                            member.MembershipType == MembershipType.New ? 
                            (
                                member.FeeOption != FeeOption.OneYearPatronage ? 
                                (
                                    "<p>Congratulations! You are now a member of AWIC! </p>" + 
                                    "<p>You can start enjoying the benefits of becoming a member right away. Some of them are: </p>"
                                ) 
                                : 
                                (
                                    "<p>Thank you for becoming a patron of AWIC! </p>" +
                                    "<p>You can start enjoying some of the membership benefits right away, like: </p>"
                                )
                            ) 
                            : 
                            (
                                member.FeeOption != FeeOption.OneYearPatronage ? 
                                (
                                    "<p>Congratulations! Your membership with AWIC has been renewed! </p>" + 
                                    "<p>You can continue to receive the benefits of being a member of AWIC. </p>"
                                ) 
                                : 
                                (
                                    "<p>Thank you! Your patronage with AWIC has been renewed! </p>" +
                                    "<p>You can continue to receive membership benefits, as before. </p>"
                                )
                            )
                        )
                    ) 
                    : 
                    (
                        (
                            member.MembershipType == MembershipType.New ? 
                            (
                                member.FeeOption != FeeOption.OneYearPatronage ? 
                                (
                                    "<p>You're just one step short of becoming a member of AWIC! </p>" + 
                                    "<p>We've yet to receive your fee payment of $" + amount + " CAD for your " + feeOption + " by " + 
                                        paymentMethod + ". " + "Once we've received it, you can start enjoying the following benefits " + 
                                        "of becoming a member: </p>"
                                ) 
                                : 
                                (
                                    "<p>You're just one step short of becoming a patron of AWIC! </p>" +
                                    "<p>We've yet to receive your fee payment of $" + amount + " CAD for your " + feeOption + " by " +
                                        paymentMethod + ". " + "Once we've received it, you can start enjoying some of the membership benefits, " + 
                                        "such as: </p>"
                                )
                            ) 
                            : 
                            (
                                member.FeeOption != FeeOption.OneYearPatronage ? 
                                (
                                    "<p>You're just one step short of renewing your membership with AWIC! </p>" +
                                    "<p>We've yet to receive your fee payment of $" + amount + " CAD for your " + feeOption + " by " +
                                        paymentMethod + ". " + "Once we've received it, you can continue to receive the benefits of being " + 
                                        "a member of AWIC. </p>"
                                ) 
                                : 
                                (
                                    "<p>You're just one step short of renewing your patronage with AWIC! </p>" +
                                    "<p>We've yet to receive your fee payment of $" + amount + " CAD for your " + feeOption + " by " +
                                        paymentMethod + ". " + "Once we've received it, you can continue to receive membership benefits, " + 
                                        "as before. </p>"
                                )
                            )
                        )
                    )
                ) + 
                (
                    member.MembershipType == MembershipType.New ? 
                    (
                        "<ul>" +
                            "<li>Stand for nomination for the Board of Directors</li>" +
                            "<li>Voting privileges at the Annual General Meeting</li>" +
                            "<li>Discount on some of AWIC's events</li>" +
                            "<li>Low cost advertising in AWIC's newsletter</li>" +
                            "<li>Receive AWIC's newsletter</li>" +
                            "<li>Become connected to your community</li>" + 
                        "</ul>"
                    )
                    : 
                    (
                        ""
                    )
                ) + 
                "<p>We might contact you if we need to clarify something on the form you submitted" + 
                    (member.PaymentMethod != PaymentMethod.CreditCard ? " as well" : "") + ". </p>" + 
                "<p>If you have any questions or concerns about anything, feel free to reply back with your " + 
                    "message and we'll be glad to address it. You can also call Nilani Nanthan (our office " +
                    "assistant at (416) 499-4144 with your message, who will be more than glad to help you. </p>" + 
                "<br />" + 
                "<p>Sincerely, </p>" + 
                "<p>The AWIC Team</p>" + 
                "<br />" + 
                "<p>PS You can find our general contact information <a href=\"" + fullURL + "\">here</a>. </p>";

            SendEmail(member.EmailAddress, Subject, HTMLBody);
        }

        public static void SendFeePaidEmail(Member member, int feeAmount, string feeOption)
        {
            string Subject = "We've received a fee payment of $" + 
                feeAmount + " CAD " + 
                "from " + member.FirstName + " " + member.LastName;

            string HTMLBody =
                "<p>A fee payment of $" + 
                    feeAmount + " CAD " + 
                    "(for a " + feeOption + ")" + 
                    " from " + 
                    member.FirstName + " " + member.LastName + 
                    " was made by credit card to us for his/her " + 
                    (
                        member.MembershipType == MembershipType.New ? 
                            (
                                member.FeeOption != FeeOption.OneYearPatronage ?
                                    "new member registration" :
                                    "new patron registration"
                            ) :
                            (
                                member.FeeOption != FeeOption.OneYearPatronage ?
                                    "membership renewal" :
                                    "patronage renewal"
                            )
                    ) +
                    ". The fee payment should be arriving in our bank account in a few days. </p>";

            SendEmail(User.ADMIN, Subject, HTMLBody);
        }

        public static void SendDonationReceiptEmail(Donations donation)
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string path = urlHelper.Action("Contact", "Home");
            Uri fullURL = new Uri(HttpContext.Current.Request.Url, path);

            string Subject = "Thank you for your donation" + 
                ( !String.IsNullOrEmpty(donation.Donor) ? ", " + donation.Donor : "" ) + 
                "!";

            string HTMLBody = 
                "<p>Hi" + (!String.IsNullOrEmpty(donation.Donor) ? " " + donation.Donor + ", " : ", ") + "</p>" + 
                "<p>Thank you for making a donation to AWIC! </p>" + 
                "<p>Your donation of $" + donation.AmountInCAD + " CAD, made on " + 
                    donation.DonationDateAndTime.ToLongDateString() + " " + 
                    donation.DonationDateAndTime.ToShortTimeString() + " has been received by us. </p>" + 
                "<p>If you have any questions or concerns about anything (including the details of where your " + 
                    "donation money will be used by AWIC), feel free to reply back with your message and we'll be glad " + 
                    "to address it. </p>" + 
                "<br />" + 
                "<p>Sincerely, </p>" + 
                "<p>The AWIC Team</p>" + 
                "<br />" +
                "<p>PS You can find our general contact information <a href=\"" + fullURL + "\">here</a>. </p>";

            SendEmail(donation.DonorEmail, Subject, HTMLBody);
        }

        public static void SendDonationReceivedEmail(Donations donation)
        {
            string Subject = "We've received a donation of $" + donation.AmountInCAD + " CAD from " + 
                ( !String.IsNullOrEmpty(donation.Donor) ? donation.Donor : 
                    ( !String.IsNullOrEmpty(donation.DonorEmail) ? donation.DonorEmail : "an anonymous donor" ) 
                ) + 
                "!";

            string HTMLBody =
                "<p>A donation of $" + donation.AmountInCAD + " CAD" +
                    ", made on " +
                    donation.DonationDateAndTime.ToLongDateString() + " " + donation.DonationDateAndTime.ToShortTimeString() +
                    ", by " +
                    ( (!String.IsNullOrEmpty(donation.Donor) && !String.IsNullOrEmpty(donation.DonorEmail)) ? 
                            donation.Donor + " (" + donation.DonorEmail + ")" : 
                            (!String.IsNullOrEmpty(donation.DonorEmail) ? 
                                donation.DonorEmail : 
                                (!String.IsNullOrEmpty(donation.Donor) ? 
                                    donation.Donor :
                                    "an anonymous donor"
                                )
                            )
                    ) +
                    " has been made to us. The donation should be arriving in our bank account in a few days. </p>";

            SendEmail(User.ADMIN, Subject, HTMLBody);
        }

        public static void SendTestEmail(string To)
        {
            string Subject = "Test Email from The AWIC Team";

            string HTMLBody = "<h1>Hello from The AWIC Team</h1>";

            SendEmail(To, Subject, HTMLBody);
        }
    }
}