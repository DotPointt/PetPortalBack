namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс для работы с объектным хранилищем MinIO.
/// </summary>
public interface IMinioService
{
    /// <summary>
    /// Загрузить файл в объектное хранилище.
    /// </summary>
    /// <param name="fileName">Имя файла.</param>
    /// <param name="fileStream">Поток файла.</param>
    /// <param name="contentType">Тип содержимого файла.</param>
    /// <returns>Путь к файлу в объектном хранилище.</returns>
    Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType);

    /// <summary>
    /// Получить файл из объектного хранилища.
    /// </summary>
    /// <param name="fileName">Путь к файлу.</param>
    /// <returns>Поток файла.</returns>
    /// <exception cref="FileNotFoundException">Выбрасывается, если файл не найден.</exception>
    Task<MemoryStream> GetFileAsync(string fileName);
}