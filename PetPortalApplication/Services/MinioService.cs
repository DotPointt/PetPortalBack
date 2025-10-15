using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Configs;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис для работы с объектным хранилищем MinIO.
/// </summary>
public class MinioService : IMinioService
{
    /// <summary>
    /// Клиент для работы с объектным хранилищем (Amazon S3).
    /// </summary>
    private readonly IAmazonS3 _s3Client;
    
    /// <summary>
    /// Имя корзины (контейнера) в объектном хранилище.
    /// </summary>
    private readonly string _bucketName;

    /// <summary>
    /// Имя файла по умолчанию для аватара.
    /// </summary>
    private readonly string _defaultKey;
    
    /// <summary>
    /// Конструктор сервиса.
    /// </summary>
    /// <param name="options">Конфигурация MinIO.</param>
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
    
    
    public async Task EnsureBucketExistsAsync()
    {
        try
        {
            await _s3Client.GetBucketLocationAsync(new GetBucketLocationRequest
            {
                BucketName = _bucketName
            });
            Console.WriteLine($"Bucket '{_bucketName}' уже существует.");
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"Bucket '{_bucketName}' не найден. Создаём...");
            await _s3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = _bucketName
            });
            Console.WriteLine($"Bucket '{_bucketName}' успешно создан.");
        }
    }
    
    
    /// <summary>
    /// Сделать bucket публичным для чтения (анонимный доступ к объектам).
    /// </summary>
    public async Task MakeBucketPublicAsync()
    {
        var policy = new
        {
            Version = "2012-10-17",
            Statement = new[]
            {
                new
                {
                    Effect = "Allow",
                    Principal = "*",
                    Action = new[] { "s3:GetObject" },
                    Resource = $"arn:aws:s3:::{_bucketName}/*"
                }
            }
        };

        var policyJson = JsonSerializer.Serialize(policy, new JsonSerializerOptions
        {
            WriteIndented = false
        });

        await _s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest
        {
            BucketName = _bucketName,
            Policy = policyJson
        });

        Console.WriteLine($"Bucket '{_bucketName}' настроен как публичный для чтения.");
    }
    
    
    /// <summary>
    /// Загрузка файла в объектное хранилище.
    /// </summary>
    /// <param name="fileName">Имя файла.</param>
    /// <param name="fileStream">Поток файла.</param>
    /// <param name="contentType">Тип содержимого файла.</param>
    /// <returns>Имя файла в объектном хранилище.</returns>
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
    /// Получение файла из объектного хранилища.
    /// </summary>
    /// <param name="fileName">Имя файла.</param>
    /// <returns>Поток файла.</returns>
    /// <exception cref="FileNotFoundException">Файл не найден.</exception>
    public async Task<MemoryStream> GetFileAsync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new Exception("Название файла не может быть пустым.");
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
            throw new FileNotFoundException($"Файл {fileName} не найден в корзине {_bucketName}.");
        }
    }
}