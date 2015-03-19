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
            string Subject = "New Volunteer Application";

            string HTMLBody = 
                "<p>An application was submitted by " + volunteer.Name + " to become a volunteer at AWIC. </p>" + 
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
                "message and we'll be glad to address it. </p>" +
                "<br />" +
                "<p>Sincerely, </p>" +
                "<p>The AWIC Team</p>" +
                "<br />" +
                "<p>PS You can find our full contact information <a href=\"" + fullURL + "\">here</a>. </p>";

            SendEmail(volunteer.EmailAddress, Subject, HTMLBody);
        }

        public static void SendMemberRegistrationEmail(Member member)
        {
            string Subject = "";

            string HTMLBody = "";

            SendEmail(User.ADMIN, Subject, HTMLBody);
        }

        public static void SendMemberRegistrationReceivedEmail(Member member)
        {
            string Subject = "";

            string HTMLBody = "";

            //SendEmail(member.EmailAddress, Subject, HTMLBody);
        }

        public static void SendDonationReceiptEmail(Donations donation)
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string path = urlHelper.Action("Contact", "Home");
            Uri fullURL = new Uri(HttpContext.Current.Request.Url, path);

            string Subject = "Thank you for your donation!";

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
                "<p>PS You can find our full contact information <a href=\"" + fullURL + "\">here</a>. </p>";

            SendEmail(donation.DonorEmail, Subject, HTMLBody);
        }

        public static void SendDonationReceivedEmail(Donations donation)
        {
            string Subject = "We've received a donation!";

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