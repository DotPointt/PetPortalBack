using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
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

    private readonly IMailSenderService _emailService;

    private readonly IPasswordHasher _passwordHasher;

    private readonly IResetPasswordService _resetPasswordService;

        
    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="userService">Сервис для работы с пользователями.</param>
    public AuthorizationController(IUserService userService, IMailSenderService emailService, IPasswordHasher passwordHasher, IResetPasswordService resetPasswordService)
    {
        _userService = userService;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _resetPasswordService = resetPasswordService;
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
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(30),
                Domain = "localhost"
            };

            HttpContext.Response.Cookies.Append("jwttoken", token, cookieOptions);

            return Ok(new { UserId = userId, Token = token });
        }
        catch (InvalidOperationException ex)
        {
            // 409
            return StatusCode(StatusCodes.Status409Conflict, 
                new { Message = "Пользователь с такой почтой уже существует."});
        }
        catch (ArgumentException ex)
        {
            // 400 
            return StatusCode(StatusCodes.Status400BadRequest, 
                new { Message = ex.Message });
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

            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(30),
                Domain = "localhost"
            };

            HttpContext.Response.Cookies.Append("jwttoken", token, cookieOptions);

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

        var request = HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}/api/Authorization/ResetPassword";

        ///генерация восстановительнйо ссылки и токена в ней
        var token = _resetPasswordService.GenerateResetPasswordToken(32);
        var url = _resetPasswordService.GeneratePasswordResetLink(baseUrl, token, user.Id);
        
        
        //Хэширование токена и сохранение в БД
        var hashedToken = _passwordHasher.HashPassword(token);
        var id = Guid.NewGuid();
        await _resetPasswordService.SaveTokenHash(ResetPasswordTokens.Create(id, user.Id, hashedToken, DateTime.UtcNow.ToUniversalTime().AddDays(1)));

        //Сгенерить адекватное письмо( добавить текста)
        //отправка восстановительной ссылки
        await _emailService.SendEmailAsync(user.Email, "Восстановление пароля", url);

        return Ok();
    }
    
    /// <summary>
    ///
    /// </summary>
    /// <param name="code">Код из url ссылки на восстановление с почты</param>
    /// <returns></returns>
    [HttpPost("ResetPassword")]
    public async Task<ActionResult> ResetPassword( string token, string userId ,string newPassword1, string newPassword2)
    {
        ///Сброс пароля 
        if (token == null)
            return BadRequest(new { error = "Ошибка: Срок действия токена истёк, или был получен новый токен." });
        
        if (newPassword1 != newPassword2)
            return BadRequest(new { error = "Ошибка: Пароли не совпадают!" });

        //сравнить хэш токена пришедшего с хэшем токена из бд
        var dbTokenHash = await _resetPasswordService.GetTokenHashByUserId(new Guid(userId));
        var isValidToken = _passwordHasher.VerifyHashedPassword(dbTokenHash.TokenHash ,token);

        if (isValidToken)
        {
            await _userService.UpdatePasswordByIdAsync(new Guid(userId), newPassword1);
            return Ok();
        }

        return BadRequest(new { error = "Ошибка: Срок действия токена истёк!" });
    }

    /// <summary>
    /// Получение информации о текущем пользователе.
    /// </summary>
    /// <param name="userData">Данные о пользователе.</param>
    /// <returns>Информация о текущем пользователе.</returns>
    [HttpPut("ChangeProfileData")]
    [Authorize]
    public async Task<ActionResult> ChangeProfileData(UserDto userData)
    {
        try
        {
            var userIdClaim = User.FindFirst("sub") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("Идентификатор пользователя не найден в токене.");
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new UnauthorizedAccessException("Неверный формат идентификатора пользователя.");
            }
            
            var user = await _userService.GetUserById(userId);

            if (user.Id != userData.Id)
            {
                return BadRequest(new { error = "Изменять можно только свой профиль." });
            }

            var id = await _userService.Update(userData);
            
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
   
    /// <summary>
    /// Получение информации о текущем пользователе.
    /// </summary>
    /// <returns>Информация о текущем пользователе.</returns>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult> GetCurrentUser()
    {
        try
        {
            var userIdClaim = User.FindFirst("sub") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("Идентификатор пользователя не найден в токене.");
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new UnauthorizedAccessException("Неверный формат идентификатора пользователя.");
            }
            
            var user = await _userService.GetUserById(userId);
            
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /*
    /// <summary>
    /// Получение текущего пользователя.
    /// </summary>
    /// <returns>Текущего пользователя.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Идентификатор пользователя не найден в токене, или
    /// неверный формат идентификатора пользователя.
    /// </exception>
    public async Task<User> CurrentUser()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("Идентификатор пользователя не найден в токене.");
        }

        if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            throw new UnauthorizedAccessException("Неверный формат идентификатора пользователя.");
        }
        
        return await _userService.GetUserById(userId);
    }
    */
}