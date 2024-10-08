using System.Net;
using System.Net.Mail;

namespace MyTrades.WebApi;
public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<EmailService> logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            this.logger = logger;
            var parentSection = configuration.GetSection("Smtp");
            var smtpHost = parentSection.GetValue<string>("Host");
            var smtpPort = parentSection.GetValue<int>("Port");
            var username = parentSection.GetValue<string>("UserName");
            var password = parentSection.GetValue<string>("Password");

            
            Console.WriteLine($"Host: {smtpHost}, Port: {smtpPort}");
            Console.WriteLine($"Host: {username}, Port: {password}");
            
            
            _smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mailMessage = new MailMessage("kmanne90@gmail.com", to, subject, body);

            await _smtpClient.SendMailAsync(mailMessage);
        }
}

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}