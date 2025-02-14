using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using PetPortalApplication.Services;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Avatar controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AvatarController : ControllerBase
{
    /// <summary>
    /// MinIO service.
    /// </summary>
    private readonly IMinioService _minioService;
    
    /// <summary>
    /// User service.
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// File controller constructor.
    /// </summary>
    /// <param name="minioService">MinIO service.</param>
    /// <param name="userService">User service.</param>
    public AvatarController(IMinioService minioService, IUserService userService)
    {
        _minioService = minioService;
        _userService = userService;
    }
    
    /// <summary>
    /// Upload user avatar.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="avatar">Photo.</param>
    /// <returns>FilePath.</returns>
    [HttpPost("upload-avatar/{userId}")]
    public async Task<ActionResult<string>> UploadAvatar(Guid userId, IFormFile avatar)
    {
        if (avatar.Length == 0)
            return BadRequest("Файл не загружен.");

        var fileName = $"{userId}_{avatar.FileName}";
        
        var fileUrl = await _minioService.UploadFileAsync(fileName, avatar.OpenReadStream(), avatar.ContentType);

        await _userService.UpdateAvatar(new UserDto()
        {
            Id = userId,
            AvatarUrl = fileUrl,
        });

        return Ok(fileUrl);
    }

    /// <summary>
    /// Get file from object storage.
    /// </summary>
    /// <param name="fileName">File path</param>
    /// <returns>File stream from</returns>
    [HttpGet("download/{fileName}")]
    public async Task<ActionResult> GetFile(string fileName)
    {
        try
        {
            var fileStream = await _minioService.GetFileAsync(fileName);
            return File(fileStream, "application/octet-stream", fileName);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Upload file.
    /// </summary>
    /// <param name="file">File.</param>
    /// <returns>File path.</returns>
    [HttpPost("upload")]
    public async Task<ActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не загружен.");

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var fileUrl = await _minioService.UploadFileAsync(fileName, file.OpenReadStream(), file.ContentType);
        
        return Ok(fileUrl);
    }
}