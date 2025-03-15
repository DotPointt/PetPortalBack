using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs.Requests;
using Exception = System.Exception;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Контроллер для авторизации и регистрации пользователей.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    /// <summary>
    /// Сервис для работы с пользователями.
    /// </summary>
    private readonly IUserService _userService;
    
    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public AuthorizationController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="request">Данные пользователя для регистрации.</param>
    /// <returns>
    /// Возвращает идентификатор созданного пользователя и токен аутентификации.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserContract request)
    {
        try
        {
            var userId = await _userService.Register(request);
            var token = await _userService.Login(request.Email, request.Password);

            // Устанавливаем токен в cookies
            HttpContext.Response.Cookies.Append("jwttoken", token);

            return Ok(new { UserId = userId, Token = token });
        }
        catch (InvalidOperationException ex)
        {
            // 409
            return Conflict(new { Message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            // 400 
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            // 500
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Message = "Произошла внутренняя ошибка сервера." });
        }
    }

    /// <summary>
    /// Аутентификация пользователя.
    /// </summary>
    /// <param name="request">Данные для входа (email и пароль).</param>
    /// <returns>
    /// Возвращает токен аутентификации.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLoginRequest request)
    {
        try
        {
            var token = await _userService.Login(request.Email, request.Password);

            // Устанавливаем токен в cookies
            HttpContext.Response.Cookies.Append("jwttoken", token);

            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            // 401
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            // 500
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Message = "Произошла внутренняя ошибка сервера." });
        }
    }
}