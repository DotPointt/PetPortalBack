using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using PetPortalCore.Configs;
using PetPortalCore.Abstractions.Services;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис отправки писем почтой. 
/// </summary>
public class MailSenderService : IMailSenderService
{
    /// <summary>
    /// SMTP Email конфигурация. 
    /// </summary>
    private readonly EmailConfig _emailSettings;

    /// <summary>
    /// Конструктор сервиса отправки писем.
    /// </summary>
    /// <param name="smtpSettings">SMTP конфигурация.</param>
    public MailSenderService(IOptions<EmailConfig> smtpSettings)
    {
        _emailSettings = smtpSettings.Value;
    }

    /// <summary>
    /// Отправляет mail письмо.
    /// </summary>
    /// <param name="to">Почта получателя.</param>
    /// <param name="subject">Объект отправки.</param>
    /// <param name="body">Письмо.</param>
    /// <exception cref="Exception"></exception>
    public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = false)
    {
        try
        {
            using var mail = new MailMessage();
            using var smtpClient = new SmtpClient();
            
            mail.From = new MailAddress(_emailSettings.From, _emailSettings.UserName);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isBodyHtml;

            smtpClient.Host = _emailSettings.Host;
            smtpClient.Port = _emailSettings.Port;
            smtpClient.EnableSsl = _emailSettings.EnableSsl;
            smtpClient.UseDefaultCredentials = false; 
            smtpClient.Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            await smtpClient.SendMailAsync(mail);
        }
        catch (SmtpException ex)
        {
            throw new ApplicationException("Ошибка при отправке письма через SMTP.", ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Ошибка при отправке письма.", ex);
        }
    }
}