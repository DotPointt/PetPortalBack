using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Контроллер для управления участниками проектов.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MembersController : ControllerBase
{
    /// <summary>
    /// Сервис для работы с участниками проектов.
    /// </summary>
    private readonly IUserProjectService _membersService;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="membersService">Сервис для работы с участниками проектов.</param>
    public MembersController(IUserProjectService membersService)
    {
        _membersService = membersService;
    }

    /// <summary>
    /// Получить список участников проекта.
    /// </summary>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>
    /// Список участников проекта.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetProjectMembers([FromBody] Guid projectId)
    {
        try
        {
            var members = await _membersService.GetProjectMembers(projectId);
            
            var response = members
                .Select(p =>
                    new UserDto()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Email = p.Email,
                        RoleId = p.RoleId,
                        AvatarUrl = p.AvatarUrl,
                        Country = p.Country,
                        City = p.City,
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
    /// Добавить нового участника в проект.
    /// </summary>
    /// <param name="member">Данные участника.</param>
    /// <returns>
    /// Идентификатор добавленного участника.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> AddProjectMember([FromBody] MemberContract member)
    {
        try
        {
            var memberId = await _membersService.AddProjectMember(member.UserId, member.ProjectId);
            
            return Ok(memberId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Удалить участника из проекта.
    /// </summary>
    /// <param name="member">Данные участника.</param>
    /// <returns>
    /// Идентификатор удаленного участника.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpDelete]
    public async Task<ActionResult<Guid>> RemoveProjectMember([FromBody] MemberContract member)
    {
        try
        {
            var memberId = await _membersService.DeleteProjectMember(member.UserId, member.ProjectId);
            
            return Ok(memberId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}