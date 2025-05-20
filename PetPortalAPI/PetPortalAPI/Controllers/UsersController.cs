using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PetPortalApplication.Services;
using PetPortalCore.DTOs.Requests;
using PetPortalCore.Models;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Контроллер для управления пользователями.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    /// <summary>
    /// Сервис для работы с пользователями.
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// Провайдер для работы с JWT-токенами.
    /// </summary>
    private readonly IJwtProvider _jwtProvider;

    /// <summary>
    /// Сервис для работы с объектным хранилищем MinIO.
    /// </summary>
    private readonly IMinioService _minioService;
    
    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    /// <param name="jwtProvider">Провайдер для работы с JWT-токенами.</param>
    /// <param name="minioService">Сервис для работы с объектным хранилищем.</param>
    public UsersController(IUserService userService, IJwtProvider jwtProvider, IMinioService minioService)
    {
        _userService = userService;
        _jwtProvider = jwtProvider;
        _minioService = minioService;
    }
        
    /// <summary>
    /// Получить список всех пользователей.
    /// </summary>
    /// <returns>
    /// Список пользователей.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {  
        try
        {
            var users = await _userService.GetAll();
            
            var response = users
                .Select(p => 
                    new UserDto()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Email = p.Email,
                        PasswordHash = p.PasswordHash,
                        AvatarUrl = p.AvatarUrl,
                        City = p.City,
                        Country = p.Country,
                        Phone = p.Phone,
                        Telegram = p.Telegram,
                    }
                );

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Получить проекты, созданные текущим пользователем.
    /// </summary>
    /// <returns>
    /// Список проектов.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpGet("MyProjects")]
    public async Task<ActionResult<List<Project>>> GetUserProjects()
    {
        try
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("Идентификатор пользователя не найден в токене.");
            }
        
            var userId = Guid.Parse(userIdClaim);
            var projects = await _userService.GetOwnProjects(userId);
        
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
    
    /// <summary>
    /// Получить защищенные данные (требуется авторизация).
    /// </summary>
    /// <returns>Защищенные данные.</returns>
    [Authorize] 
    [HttpGet("data")]
    public ActionResult<string> Data()
    {
        return "Получены защищенные данные.";
    }

    /// <summary>
    /// Тестовый метод для проверки данных пользователя из контекста.
    /// </summary>
    /// <returns>Данные пользователя из контекста.</returns>
    [HttpGet("testreply")]
    public ActionResult<string> TestReply()
    {
        var user = HttpContext.User;
        
        return Ok(user);
    }

    /// <summary>
    /// Обновить данные пользователя.
    /// </summary>
    /// <param name="request">Обновленные данные пользователя.</param>
    /// <returns>
    /// Идентификатор обновленного пользователя.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UserDto request)
    {
        try
        {
            var id = await _userService.Update(request);
            
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
        
    /// <summary>
    /// Удалить пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>
    /// Идентификатор удаленного пользователя.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteUser([FromBody] Guid id)
    {
        try
        {
            var userId = await _userService.Delete(id);

            return Ok(userId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }    
    }
}