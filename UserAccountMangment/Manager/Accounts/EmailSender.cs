
using Microsoft.Extensions.Options;
using System.Net.Mail;
using UserAccountMangment.Dtos.AccountDtos;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
namespace UserAccountMangment.Manager.Accounts
{
    public class EmailSender : IEmailSender
    {
        private readonly string _sendGridApiKey;

        public EmailSender(string sendGridApiKey)
        {
            _sendGridApiKey = sendGridApiKey;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(_sendGridApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("your-email@example.com", "Your Name"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            var response = await client.SendEmailAsync(msg);
            // You may handle response if needed (e.g., check for errors)
        }
    }
}


