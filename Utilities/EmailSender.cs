using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions emailOptions;
        public EmailSender(IOptions<EmailOptions> options)
        {
            this.emailOptions = options.Value;
        }
        public Task SendEmailAsync(string toEmailAddress, string subject, string htmlMessage)
        {
           return Execute(emailOptions.SendGridKey, emailOptions.FromAddress, toEmailAddress, subject, htmlMessage);
        }
        private static Task Execute(string sendGridKey, string fromEmailAddress, string toEmailAddress, string subject, string htmlMessage)
        {
            var client = new SendGridClient(sendGridKey);
            var fromAddress = new EmailAddress(fromEmailAddress, "rsvalkyrie8@gmail.com");
            var toAddress = new EmailAddress(toEmailAddress, "");
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, "", htmlMessage);
            var response = client.SendEmailAsync(message);
            return response;
        }
    }
}
