namespace PetPortalCore.Configs;

/// <summary>
/// Конфигурация для работы с объектным хранилищем MinIO.
/// </summary>
public class MinIOConfig
{
    /// <summary>
    /// URL сервиса MinIO.
    /// </summary>
    public string ServiceUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Ключ доступа к MinIO.
    /// </summary>
    public string AccessKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Секретный ключ для доступа к MinIO.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Имя корзины (контейнера) в MinIO.
    /// </summary>
    public string BucketName { get; set; } = string.Empty;
    
    /// <summary>
    /// Имя файла по умолчанию для аватара.
    /// </summary>
    public string DefaultKey { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;
}