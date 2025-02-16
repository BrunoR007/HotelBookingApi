using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Habitus.Services;

public class EmailSender : IEmailSender
{
    private readonly string _smtpServer = "smtp.yourmailserver.com";
    private readonly int _smtpPort = 587; 
    private readonly string _smtpUser = "youremail@domain.com"; 
    private readonly string _smtpPassword = "yourpassword";

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var client = new SmtpClient(_smtpServer, _smtpPort);
        client.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
        client.EnableSsl = true;

        MailMessage mailMessage = new()
        {
            From = new MailAddress(_smtpUser),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);

        await client.SendMailAsync(mailMessage);
    }
}