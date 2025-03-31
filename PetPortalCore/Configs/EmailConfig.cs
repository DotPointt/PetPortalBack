namespace PetPortalCore.Configs;

/// <summary>
/// Конфигурация почты.
/// </summary>
public class EmailConfig
{
    /// <summary>
    /// Хост сервиса.
    /// </summary>
    public string Host { get; set; }
    
    /// <summary>
    /// Порт сервиса.
    /// </summary>
    public int Port { get; set; }
    
    /// <summary>
    /// Нужно ли использовать SSL/TLS для
    /// шифрования соединения при отправке почты через SMTP.
    /// </summary>
    public bool EnableSsl { get; set; }
    
    /// <summary>
    /// Имя владельца почты.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Пароль почты.
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Почта отправителя.
    /// </summary>
    public string From { get; set; }
}