namespace PetPortalCore.Configs;

/// <summary>
/// MinIO service configuration.
/// </summary>
public class MinIOConfig
{
    /// <summary>
    /// Service path.
    /// </summary>
    public string ServiceUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Access key.
    /// </summary>
    public string AccessKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Secret key.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Bucket name.
    /// </summary>
    public string BucketName { get; set; } = string.Empty;
}