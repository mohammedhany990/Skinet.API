using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Skinet.Core.Interfaces;

namespace Skinet.Repository.Email
{
    public class EmailSettings : IEmailSettings
    {
        private MailSettings _options;

        public EmailSettings(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }

        public void SendEmail(Core.Entities.Email email)
        {

            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject
            };
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();



            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);
            smtp.Send(mail);
            smtp.Disconnect(true);

        }

        /*
        private MailSettings _options;

        public EmailSettings(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }

        
        public void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,
            };

            mail.To.Add(MailboxAddress.Parse(email.To));


            // Build the email body with HTML content
            var builder = new BodyBuilder
            {
                HtmlBody = email.Body,  // Use HtmlBody for HTML content
                TextBody = "This is a plain text version of the email." // Optional: fallback text
            };

            

            mail.Body = builder.ToMessageBody();

            // To put the DisplayName not sender's email
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

            // To Connect to Mail Provider -> smtp
            using var smtp = new SmtpClient();

            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);

            smtp.Authenticate(_options.Email, _options.Password);

            smtp.Send(mail);

            smtp.Disconnect(true);

        }

        */
    }
}
