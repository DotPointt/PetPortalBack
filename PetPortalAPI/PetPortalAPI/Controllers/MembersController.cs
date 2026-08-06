using Microsoft.AspNetCore.Authorization;
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
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IUserProjectService _membersService;

    public MembersController(IUserProjectService membersService)
    {
        _membersService = membersService;
    }

    [HttpGet("{projectId:guid}")]
    public async Task<ActionResult<List<UserDto>>> GetProjectMembers(Guid projectId)
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
