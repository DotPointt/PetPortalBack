using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PetPortalAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IUserService _userService;
    private readonly IProjectsService _projectsService;

    public PaymentController(
        IPaymentService paymentService,
        IUserService userService,
        IProjectsService projectsService)
    {
        _paymentService = paymentService;
        _userService = userService;
        _projectsService = projectsService;
    }

    /// <summary>
    /// Создать платёж за размещение уже созданного (Closed) проекта.
    /// </summary>
    [SwaggerOperation(Summary = "Оплата размещения проекта")]
    [HttpPost("placement/{projectId:guid}")]
    [Authorize]
    public async Task<IActionResult> CreatePlacementPayment(Guid projectId)
    {
        try
        {
            var userId = await _userService.GetUserIdFromJWTAsync(User);
            if (userId == null)
                return Unauthorized();

            var project = await _projectsService.GetById(projectId);
            if (project.OwnerId != userId.Value)
                return Forbid();

            if (project.StateOfProject == PetPortalCore.Models.StateOfProject.Open)
                return BadRequest("Проект уже опубликован.");

            var url = await _paymentService.CreatePlacementPaymentAsync(projectId, userId.Value);
            return Ok(new { paymentUrl = url, projectId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Подтвердить оплату по id платежа YooKassa (для return URL / ручной проверки).
    /// </summary>
    [HttpPost("confirm/{paymentId}")]
    [Authorize]
    public async Task<IActionResult> Confirm(string paymentId)
    {
        try
        {
            var ok = await _paymentService.ConfirmPaymentAndPublishAsync(paymentId);
            return Ok(new { paid = ok });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
