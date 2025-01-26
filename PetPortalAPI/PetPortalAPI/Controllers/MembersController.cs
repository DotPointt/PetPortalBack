using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Projects controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MembersController : Controller
{
    /// <summary>
    /// Members service.
    /// </summary>
    private readonly IUserProjectService _membersService;

    /// <summary>
    /// Members controller constructor.
    /// </summary>
    /// <param name="membersService">Members service.</param>
    public MembersController(IUserProjectService membersService)
    {
        _membersService = membersService;
    }

    /// <summary>
    /// Get project members.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>
    /// Action result - List of users or
    /// Action result - error message.
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
                        Password = p.PasswordHash,
                        RoleId = p.RoleId,
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
    /// Add new project member.
    /// </summary>
    /// <param name="member">Member data.</param>
    /// <returns>New member identifier.</returns>
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
    /// Remove project member.
    /// </summary>
    /// <param name="member">Member data.</param>
    /// <returns>Deleted member identifier.</returns>
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
    