using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Configs;

namespace PetPortalApplication.Services;

/// <summary>
/// Object storage service via minIO.
/// </summary>
public class MinioService : IMinioService
{
    /// <summary>
    /// Object storage client.
    /// </summary>
    private readonly IAmazonS3 _s3Client;
    
    /// <summary>
    /// Object storage bucket name.
    /// </summary>
    private readonly string _bucketName;

    /// <summary>
    /// The name of the default avatar bucket
    /// </summary>
    private readonly string _defaultKey;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="options">Configs.</param>
    public MinioService(IOptions<MinIOConfig> options)
    {
        var config = options.Value;

        _s3Client = new AmazonS3Client(
            config.AccessKey,
            config.SecretKey,
            new AmazonS3Config
            {
                ServiceURL = config.ServiceUrl,
                ForcePathStyle = true
            });

        _bucketName = config.BucketName;
        _defaultKey = config.DefaultKey;
    }
    
    /// <summary>
    /// Upload file on object storage.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="fileStream">File stream.</param>
    /// <param name="contentType">Content type.</param>
    /// <returns>File name on object storage.</returns>
    public async Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType
            };

            await _s3Client.PutObjectAsync(request);

            return fileName;
        }
        catch (AmazonS3Exception ex)
        {
            return ex.ToString();
        }
    }
    
    /// <summary>
    /// Get file from object storage.
    /// </summary>
    /// <param name="fileName">File path.</param>
    /// <returns>File stream.</returns>
    /// <exception cref="FileNotFoundException">File not existed.</exception>
    public async Task<MemoryStream> GetFileAsync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            fileName = _defaultKey; // Заменяем пустое значение на дефолтное
        }
        
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            
            using var response = await _s3Client.GetObjectAsync(request);
            
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new FileNotFoundException($"Файл {fileName} не найден в бакете {_bucketName}.");
        }
    }
}