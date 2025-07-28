using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Контроллер работы откликов.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RespondsController : ControllerBase
{
    /// <summary>
    /// Сервис работы откликов.
    /// </summary>
    private readonly IRespondService _respondService;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="respondService">Сервис работы откликов.</param>
    public RespondsController(IRespondService respondService)
    {
        _respondService = respondService;
    }

    /// <summary>
    /// Получить все отклики.
    /// </summary>
    /// <returns>Список всех откликов</returns>
    [HttpGet("AllResponds")]
    public async Task<ActionResult<List<RespondDto>>> GetAllResponds()
    {
        try
        {
            var responds = await _respondService.GetAllResponds();
            
            return Ok(responds);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
    
    /// <summary>
    /// Получить все отклики от пользователя.
    /// </summary>
    /// <returns>Список всех откликов</returns>
    [HttpGet("RespondsByUser/{userId}")]
    public async Task<ActionResult<List<RespondDto>>> GetRespondsByUserId(Guid userId)
    {
        try
        {
            var responds = await _respondService.GetRespondsByUserId(userId);
            
            return Ok(responds);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
    
    /// <summary>
    /// Получить все отклики по проекту.
    /// </summary>
    /// <returns>Список всех откликов</returns>
    [HttpGet("RespondsByProject/{projectId}")]
    public async Task<ActionResult<List<RespondDto>>> GetRespondsByProjectId(Guid projectId)
    {
        try
        {
            var responds = await _respondService.GetRespondsByProjectId(projectId);
            
            return Ok(responds);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Создать новый отклик.
    /// </summary>
    /// <param name="respondDto">ДТО отклика.</param>
    /// <returns>Булево значение операции.</returns>
    [HttpPost("AddRespond")]
    public async Task<ActionResult<bool>> CreateRespond([FromBody] RespondCreateContract respondCreateContract)
    {
        try
        {
            var id = Guid.NewGuid();
            var respondDto = new RespondDto()
            {
                Id = id,
                Role = respondCreateContract.Role,
                Comment = respondCreateContract.Comment,
                UserId = respondCreateContract.UserId,
                ProjectId = respondCreateContract.ProjectId,
            };
            
            var res = await _respondService.CreateRespond(respondDto);

            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Удалить отклик.
    /// </summary>
    /// <param name="respondId">Идентификатор отклика.</param>
    /// <returns>Булево значение операции.</returns>
    [HttpDelete("DeleteRespond/{respondId}")]
    public async Task<ActionResult<bool>> DeleteRespond(Guid respondId)
    {
        try
        {
            var res = await _respondService.DeleteRespond(respondId);

            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}