using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AdditionalTools.EmailSender
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string to, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("eshop_cars@outlook.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = message
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp-relay.brevo.com", 587, MailKit.Security.SecureSocketOptions.StartTls);


            await smtp.AuthenticateAsync("8d844d001@smtp-brevo.com", "7aWThLqdpxU4DjJm");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
