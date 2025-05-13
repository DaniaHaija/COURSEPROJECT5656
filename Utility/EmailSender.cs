
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace COURSEPROJECT.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("haija.dania2020@gmail.com", "hahe wavs vrhi opgb\r\n")
            };

            return client.SendMailAsync(
                new MailMessage(from: "haija.dania2020@gmail.com",
                                to: email,
                                subject,
                                message
                                )

                {
                    IsBodyHtml = true
                }
                );
        }
    }
}
