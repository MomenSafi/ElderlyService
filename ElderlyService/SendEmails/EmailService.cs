using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
namespace ElderlyService.SendEmails
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Elderly Health Care", "info.careforelderly@gmail.com"));
            message.To.Add(new MailboxAddress("Recipient", to)); 
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("info.careforelderly@gmail.com", "cljk huah ieag tbud");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
