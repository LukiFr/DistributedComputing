using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Notifications.Services
{
    public class EmailSender
    {
        public void SendNewUserEmail(string email)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("lukaszfraniewskiwwsi@gmail.com", "Wwsi123#"),
                EnableSsl = true,
            };

            smtpClient.Send("lukaszfraniewskiwwsi@gmail.com", email, "COVID19", "Wiadomosc o kwarantannie");
        }
    }
}
