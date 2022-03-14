using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//223
namespace Spice.Service
{
    public class EmailSender : IEmailSender
    {
        public EmailOptions Options { get; set; }       
        public EmailSender(IOptions<EmailOptions>emailOptions)
        {
            Options = emailOptions.Value;   //getting all of emailOptions using dependency injection
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        private Task Execute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("rozaroza.nh@gmail.com", "Spice"),
                Subject = subject,
                PlainTextContent=message,
                HtmlContent=message,
            };
            msg.AddTo(new EmailAddress(email));
            try
            {
                return client.SendEmailAsync(msg);
            }
            catch(Exception ex)
            {

            }
            return null;
        }
    }
}
