using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs.Requests;
using PetPortalCore.Models;
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
    /// 
    /// </summary>
    UserManager<User> _userManager;
    
    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public AuthorizationController(IUserService userService)
    {
        _userService = userService;
        // _userManager = userManager;
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
                new { Message = "Произошла внутренняя ошибка сервера:" });
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

    /// <summary>
    /// Страничка для сброса пароля ( поле почты на которую нужно отправить ссылку для сброса пароля )
    /// Возвращает только OK, чтобы злоумышленник не мог определить есть ли почта в бд
    /// </summary>
    /// <returns></returns>
    [HttpPost("ForgotPassword")]
    public async Task<ActionResult> ForgotPassword(string Email)
    {
        var user = await _userService.GetUserByEmail(Email);
        
        // try
        // {
        //     var user = await _userService.GetUserById(new Guid());
        // }
        // catch (Exception ex)
        // {
        //     throw (ex);
        // }
        
        if (user == null)
        {
            return Ok();
        }
        
        ///генерация восстановительнйо ссылки
        string code = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        ///отправка восстановительной ссылки\
        /// 
        
        return Ok();
    }
    
    /// <summary>
    ///
    /// </summary>
    /// <param name="code">Код из url ссылки на восстановление с почты</param>
    /// <returns></returns>
    [HttpPost("ResetPassword")]
    
    public async Task<ActionResult> ResetPassword( string code)
    {
        ///Сброс пароля  
        
        return Ok();
    }
}