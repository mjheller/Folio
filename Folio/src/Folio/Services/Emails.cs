using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;


namespace Folio.Services
{
    public static class Emails
    {
        public static string errorMessage;
        public static  void ProcessEmail(string To, string From, string Subject, string Body)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Secret.ProjectEmail, Secret.ProjectPassword);
                MailMessage msg = new MailMessage();
                msg.To.Add(To);
                msg.From = new MailAddress(From);
                msg.Subject = (Subject);
                msg.Body = Body;
                client.Send(msg);
                errorMessage = "";
            }
            catch (Exception ex)
            {
                errorMessage = "Could not email";
            }
        }

        public static string GetContactName(int? id)
        {
            if (id == 1)
            {
                return "Aaron DeSanctis";
            }
            if (id == 2)
            {
                return "Alex Morask";
            }
            if (id == 3)
            {
                return "Chad Hilke";
            }
            if (id == 4)
            {
                return "Chris Hoelter";
            }
            if (id == 5)
            {
                return "Josh Oliver";
            }
            if (id == 6)
            {
                return "Matt Heller";
            }
            else
            {
                return "Robert Moon";
            }
        }

        public static string GetContactEmailAddress(int? id)
        {
            if (id == 1)
            {
                return Secret.AaronEmail;
            }
            if (id == 2)
            {
                // add alex's email address
                return Secret.AlexEmail;
            }
            if (id == 3)
            {
                // add chad's email address
                return Secret.ChadEmail;
            }
            if (id == 4)
            {
                // add chris's email address
                return Secret.ChrisEmail;
            }
            if (id == 5)
            {
                // add josh's email address
                return Secret.JoshEmail;
            }
            if (id == 6)
            {
                return Secret.MattEmail;
            }
            else
            {
                // add robert's email address
                return Secret.RobertEmail;
            }
        }
    }
}
