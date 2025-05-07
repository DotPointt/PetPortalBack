using Microsoft.AspNetCore.Mvc;
using PetPortalApplication.Services;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Контроллер для работы с аватарами пользователей и файлами.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AvatarController : ControllerBase
{
    /// <summary>
    /// Сервис для работы с объектным хранилищем MinIO.
    /// </summary>
    private readonly IMinioService _minioService;
    
    /// <summary>
    /// Сервис для работы с пользователями.
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="minioService">Сервис MinIO для работы с файлами.</param>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public AvatarController(IMinioService minioService, IUserService userService)
    {
        _minioService = minioService;
        _userService = userService;
    }
    
    /// <summary>
    /// Загрузка аватара пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="avatar">Файл аватара.</param>
    /// <returns>URL загруженного аватара.</returns>
    [HttpPost("upload-avatar/{userId}")]
    public async Task<ActionResult<string>> UploadAvatar(Guid userId, IFormFile avatar)
    {
        if (avatar.Length == 0)
            return BadRequest("Файл не загружен.");

        // Генерация уникального имени файла
        var fileName = $"{userId}_{avatar.FileName}";
        
        // Загрузка файла в MinIO
        var fileUrl = await _minioService.UploadFileAsync(fileName, avatar.OpenReadStream(), avatar.ContentType);

        // Обновление аватара пользователя
        await _userService.UpdateAvatar(new UserDto()
        {
            Id = userId,
            AvatarUrl = fileUrl,
        });

        return Ok(fileUrl);
    }

    /// <summary>
    /// Получение файла из объектного хранилища.
    /// </summary>
    /// <param name="fileName">Имя файла.</param>
    /// <returns>Поток файла.</returns>
    [HttpGet("download/{fileName}")]
    public async Task<ActionResult> GetFile(string fileName)
    {
        try
        {
            // Получение файла из MinIO
            var fileStream = await _minioService.GetFileAsync(fileName);
            return File(fileStream, "application/octet-stream", fileName);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Загрузка файла в объектное хранилище.
    /// </summary>
    /// <param name="file">Файл для загрузки.</param>
    /// <returns>URL загруженного файла.</returns>
    [HttpPost("upload")]
    public async Task<ActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не загружен.");

        // Генерация уникального имени файла
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        
        // Загрузка файла в MinIO
        var fileUrl = await _minioService.UploadFileAsync(fileName, file.OpenReadStream(), file.ContentType);
        
        return Ok(fileUrl);
    }
}