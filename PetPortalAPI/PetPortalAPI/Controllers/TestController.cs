using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs.Requests;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Тестовый контроллер.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    /// <summary>
    /// Сервис работы с почтой.
    /// </summary>
    private readonly IMailSenderService _mailService;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="mailService">Сервис работы с почтой.</param>
    public TestController(IMailSenderService mailService)
    {
        _mailService = mailService;
    }

    /// <summary>
    /// Тест отправки mail.
    /// </summary>
    /// <param name="request">Запрос на отправку mail.</param>
    /// <returns>Стандартный ответ.</returns>
    [HttpPost("SendMail")]
    public async Task<IActionResult> SendTestEmail([FromBody] EmailSendRequest request)
    {
        try
        {
             await _mailService.SendEmailAsync(request.Email, request.Subject, request.Message);
        
            return Ok();
            
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}