using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetPortalApplication.Helpers;
using PetPortalCore.Abstractions.Repositories;
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
    private readonly IUserService _userService;
    private readonly IMailSenderService _emailService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IResetPasswordService _resetPasswordService;
    private readonly IEmailConfirmationService _emailConfirmationService;
    private readonly IEmailConfirmationTokensRepository _emailConfirmationTokensRepository;

    public AuthorizationController(
        IUserService userService,
        IMailSenderService emailService,
        IPasswordHasher passwordHasher,
        IResetPasswordService resetPasswordService,
        IEmailConfirmationService emailConfirmationService,
        IEmailConfirmationTokensRepository emailConfirmationTokensRepository)
    {
        _userService = userService;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _resetPasswordService = resetPasswordService;
        _emailConfirmationService = emailConfirmationService;
        _emailConfirmationTokensRepository = emailConfirmationTokensRepository;
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserContract request)
    {
        try
        {
            var userId = await _userService.Register(request);
            try
            {
                await SendConfirmationEmailAsync(userId, request.Email, request.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Confirmation email failed: {ex.Message}");
            }

            return Ok(new
            {
                UserId = userId,
                RequiresEmailConfirmation = true,
                Message = "На вашу почту отправлено письмо для подтверждения регистрации."
            });
        }
        catch (InvalidOperationException)
        {
            return StatusCode(StatusCodes.Status409Conflict,
                new { Message = "Пользователь с такой почтой уже существует." });
        }
        catch (ArgumentException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Произошла внутренняя ошибка сервера." });
        }
    }

    /// <summary>
    /// Подтверждение email по ссылке из письма.
    /// </summary>
    [HttpPost("VerifyEmail")]
    public async Task<ActionResult> VerifyEmail(string token, string userId)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(userId))
            return BadRequest(new { Message = "Некорректная ссылка подтверждения." });

        if (!Guid.TryParse(userId, out var parsedUserId))
            return BadRequest(new { Message = "Некорректный идентификатор пользователя." });

        try
        {
            var dbToken = await _emailConfirmationService.GetTokenHashByUserId(parsedUserId);
            if (dbToken.ExpiresAt < DateTime.UtcNow)
                return BadRequest(new { Message = "Срок действия ссылки истёк. Запросите письмо повторно." });

            var isValid = _passwordHasher.VerifyHashedPassword(dbToken.TokenHash, token);
            if (!isValid)
                return BadRequest(new { Message = "Недействительная ссылка подтверждения." });

            await _userService.ConfirmEmailAsync(parsedUserId);
            await _emailConfirmationTokensRepository.DeleteByUserIdAsync(parsedUserId);

            return Ok(new { Message = "Email успешно подтверждён. Теперь вы можете войти." });
        }
        catch (InvalidOperationException)
        {
            return BadRequest(new { Message = "Ссылка подтверждения недействительна." });
        }
    }

    /// <summary>
    /// Повторная отправка письма подтверждения.
    /// </summary>
    [HttpPost("ResendConfirmationEmail")]
    public async Task<ActionResult> ResendConfirmationEmail(string email)
    {
        var user = await _userService.FindUserByEmailAsync(email);
        if (user == null)
            return Ok(new { Message = "Если аккаунт существует, письмо будет отправлено." });

        if (await _userService.IsEmailConfirmedAsync(user.Id))
            return Ok(new { Message = "Email уже подтверждён." });

        try
        {
            await SendConfirmationEmailAsync(user.Id, user.Email, user.Name);
        }
        catch
        {
            // Не раскрываем детали SMTP
        }

        return Ok(new { Message = "Если аккаунт существует, письмо будет отправлено." });
    }

    /// <summary>
    /// Аутентификация пользователя.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLoginRequest request)
    {
        try
        {
            var token = await _userService.Login(request.Email, request.Password);
            HttpContext.Response.Cookies.Append("jwttoken", token);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Произошла внутренняя ошибка сервера." });
        }
    }

    [HttpPost("ForgotPassword")]
    public async Task<ActionResult> ForgotPassword(string Email)
    {
        var user = await _userService.GetUserByEmail(Email);

        if (user == null)
            return Ok();

        var baseUrl = $"{AppUrls.FrontendBase}/forget-password";
        var token = _resetPasswordService.GenerateResetPasswordToken(32);
        var url = _resetPasswordService.GeneratePasswordResetLink(baseUrl, token, user.Id);

        var hashedToken = _passwordHasher.HashPassword(token);
        await _resetPasswordService.SaveTokenHash(
            ResetPasswordTokens.Create(Guid.NewGuid(), user.Id, hashedToken, DateTime.UtcNow.AddDays(1)));

        await _emailService.SendEmailAsync(user.Email, "Восстановление пароля", url);

        return Ok();
    }

    [HttpPost("ResetPassword")]
    public async Task<ActionResult> ResetPassword(string token, string userId, string newPassword1, string newPassword2)
    {
        if (token == null)
            return BadRequest(new { error = "Ошибка: Срок действия токена истёк, или был получен новый токен." });

        if (newPassword1 != newPassword2)
            return BadRequest(new { error = "Ошибка: Пароли не совпадают!" });

        var dbTokenHash = await _resetPasswordService.GetTokenHashByUserId(new Guid(userId));
        var isValidToken = _passwordHasher.VerifyHashedPassword(dbTokenHash.TokenHash, token);

        if (isValidToken)
        {
            await _userService.UpdatePasswordByIdAsync(new Guid(userId), newPassword1);
            return Ok();
        }

        return BadRequest(new { error = "Ошибка: Срок действия токена истёк!" });
    }

    [HttpPut("ChangeProfileData")]
    [Authorize]
    public async Task<ActionResult> ChangeProfileData(UserDto userData)
    {
        try
        {
            var userIdClaim = User.FindFirst("sub") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Идентификатор пользователя не найден в токене.");

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                throw new UnauthorizedAccessException("Неверный формат идентификатора пользователя.");

            var user = await _userService.GetUserById(userId);

            if (user.Id != userData.Id)
                return BadRequest(new { error = "Изменять можно только свой профиль." });

            var id = await _userService.Update(userData);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult> GetCurrentUser()
    {
        try
        {
            var userId = await _userService.GetUserIdFromJWTAsync(User);
            if (userId == null)
                return Unauthorized();

            var user = await _userService.GetUserById(userId.Value);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    private async Task SendConfirmationEmailAsync(Guid userId, string email, string name)
    {
        var token = _emailConfirmationService.GenerateToken(32);
        var baseUrl = $"{AppUrls.FrontendBase}/verify-email";
        var link = _emailConfirmationService.GenerateConfirmationLink(baseUrl, token, userId);
        var hashedToken = _passwordHasher.HashPassword(token);

        await _emailConfirmationService.SaveTokenHash(
            EmailConfirmationToken.Create(Guid.NewGuid(), userId, hashedToken, DateTime.UtcNow.AddDays(1)));

        var body = EmailTemplates.ConfirmRegistration(name, link);
        await _emailService.SendEmailAsync(email, "Подтверждение регистрации — PetPortal", body, isBodyHtml: true);
    }
}
