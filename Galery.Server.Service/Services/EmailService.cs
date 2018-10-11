using Galery.Server.Service.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace Galery.Server.Service.Services
{
    /// <summary>
    ///
    /// </summary>
    public class EmailService : IEmailService
    {
        readonly string _from;
        readonly string _password;

        public EmailService(IConfiguration config)
        {
            _from = config.GetSection("emailAdreses").GetSection("AdminAdress").Value;
            _password = config.GetSection("emailAdreses").GetSection("AdmitPassword").Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", _from));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, false);
                await client.AuthenticateAsync(_from, _password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
