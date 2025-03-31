using Microsoft.Win32;

namespace PetPortalCore.DTOs.Requests;

/// <summary>
/// Запрос на отправку сообщения.
/// </summary>
public class EmailSendRequest
{
    /// <summary>
    /// Почта получателя.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Тема письма.
    /// </summary>
    public string Subject { get; set; }
    
    /// <summary>
    /// Сообщение письма.
    /// </summary>
    public string Message { get; set; }
}