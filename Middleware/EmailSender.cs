using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MimeKit.Text;

namespace WebApp.Middleware
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string? email, string? subject, string? htmlMessage)
        {
            var emails = new MimeMessage();
            string? SMTPEmail = Program.Configuration?.GetValue<string>("AppSettings:SMTPEmail");
            string? SMTPEmailPassword = Program.Configuration?.GetValue<string>("AppSettings:SMTPEmailPassword");
            string? SMTPServer = Program.Configuration?.GetValue<string>("AppSettings:SMTPServer");
            int SMTPServerPort = 587;

            emails.From.Add(MailboxAddress.Parse(SMTPEmail));
            emails.To.Add(MailboxAddress.Parse(email));
            emails.Subject = subject;
            emails.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

			// send email
			using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(SMTPServer, SMTPServerPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(SMTPEmail, SMTPEmailPassword);
            smtp.Send(emails);
            smtp.Disconnect(true);

            return Task.CompletedTask;
        }
    }
}
