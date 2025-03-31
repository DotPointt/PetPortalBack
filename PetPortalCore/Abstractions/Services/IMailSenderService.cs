namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс сервиса отправки писем.
/// </summary>
public interface IMailSenderService
{
    /// <summary>
    /// Отправляет mail письмо.
    /// </summary>
    /// <param name="to">Почта получателя.</param>
    /// <param name="subject">Объект отправки.</param>
    /// <param name="body">Письмо.</param>
    /// <param name="isBodyHtml">Письмо в html стиле.</param>
    Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = false);
    
}