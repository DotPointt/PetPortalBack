namespace PetPortalCore.Abstractions.Services;

public interface IMinioService
{
    /// <summary>
    /// Upload file on object storage.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="fileStream">File stream.</param>
    /// <param name="contentType">Content type.</param>
    /// <returns>Path to file on object storage.</returns>
    Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType);

    /// <summary>
    /// Get file from object storage.
    /// </summary>
    /// <param name="fileName">File path.</param>
    /// <returns>File stream.</returns>
    /// <exception cref="FileNotFoundException">File not existed.</exception>
    Task<Stream> GetFileAsync(string fileName);
}