using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace MyQueue.Services.AuthorizationServices
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message, MailConfig config)
        {

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", config.Mail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(config.Domain, config.Port, config.SSL);
                await client.AuthenticateAsync(config.Mail, config.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
