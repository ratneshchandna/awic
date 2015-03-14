using System;
using System.Net;
using System.Net.Mail;
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
        private EmailConfig _config { get; set; }

        protected EmailHelper(EmailConfig config)
        {
            _config = config;
        }

        protected void SendEmail(string To, string Subject, string HtmlBody)
        {
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
        public AWICEmailHelper() 
            : base(
                new EmailConfig
                {
                    From = User.ADMIN,
                    SMTPServer = new SMTPServer
                    {
                        Address = "mail5005.site4now.net",
                        Port = 8889,
                        Username = User.ADMIN,
                        Password = System.Configuration.ConfigurationManager.AppSettings["SMTPServerPassword"]
                    }
                }
            )
        {
        }

        public void SendVolunteerSignUpEmail(string To, string Name)
        {
            string Subject = "";

            string HTMLBody = "";

            SendEmail(To, Subject, HTMLBody);
        }

        public void SendMemberSignUpEmail(string To, string Name)
        {
            string Subject = "";

            string HTMLBody = "";

            SendEmail(To, Subject, HTMLBody);
        }

        public void SendVolunteerSignUpReceivedEmail(string To, string Name)
        {
            string Subject = "";

            string HTMLBody = "";

            SendEmail(To, Subject, HTMLBody);
        }

        public void SendMemberSignUpReceivedEmail(string To, string Name)
        {
            string Subject = "";

            string HTMLBody = "";

            SendEmail(To, Subject, HTMLBody);
        }

        public void SendDonationReceiptEmail(string To, Donations donation)
        {
            string Subject = "Thank you for your donation!";

            string HTMLBody = 
                "<p>Hi" + (!String.IsNullOrEmpty(donation.Donor) ? " " + donation.Donor + ", " : ", ") + "</p>" + 
                "<p>Thank you for making a donation to AWIC!</p>" + 
                "<p>Your donation of $" + donation.AmountInCAD + " CAD, made on " + donation.DonationDateAndTime.ToLongDateString() + " " + donation.DonationDateAndTime.ToShortTimeString() + " has been received by us. </p>" + 
                "<br />" + 
                "<p>Sincerely, </p>" + 
                "<p>The AWIC Team</p>";

            SendEmail(To, Subject, HTMLBody);
        }

        public void SendDonationReceivedEmail(string To, Donations donation)
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

            SendEmail(To, Subject, HTMLBody);
        }

        public void SendTestEmail(string To)
        {
            string Subject = "Test Email from The AWIC Team";

            string HTMLBody = "<h1>Hello from The AWIC Team</h1>";

            SendEmail(To, Subject, HTMLBody);
        }
    }
}