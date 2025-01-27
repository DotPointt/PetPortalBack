using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PetPortalCore.DTOs.Contracts;
using Microsoft.AspNetCore.Authorization;
using PetPortalCore.DTOs.Requests;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Users controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    /// <summary>
    /// Users service.
    /// </summary>
    private readonly IUserService _userService;

    private readonly IJwtProvider _jwtProvider;

    /// <summary>
    /// Users controller constructor.
    /// </summary>
    /// <param name="userService"></param>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
        
    /// <summary>
    /// Endpoint get users.
    /// </summary>
    /// <returns>
    /// Action result - List of users or
    /// Action result - error message.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {  
        try
        {
            var users = await _userService.GetAll();
            var response = users
                .Select(p => 
                    new UserDto()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Email = p.Email,
                        Password = p.PasswordHash
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
    /// Endpoint create user.
    /// </summary>
    /// <param name="request">User data.</param>
    /// <returns>
    /// Action result - created user guid or
    /// Action result - error message.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] UserContract request)
    {
        try
        {
            var userGuid = await _userService.Register(request);

            return Ok(userGuid);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// TO access this url Header Authorization:"BearerHere" needed
    /// </summary>
    /// <returns></returns>
    [Authorize] 
    [HttpGet("data")]
    public  ActionResult<string> Data()
    {
        return "sensitive data recieved";
    }

    [HttpGet("testreply")]
    public ActionResult<string> testreply()
    {
        var user = HttpContext.User;
        return Ok(user);
    }

    /// <summary>
    /// Endpoint update user.
    /// </summary>
    /// <param name="request">User data.</param>
    /// <returns>
    /// Action result - updated user guid or
    /// Action result - error message.
    /// </returns>
    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateUser([FromBody] UserDto request)
    {
        try
        {
            var userId = await _userService.Update(
                new UserDto()
                {
                    Id = request.Id, 
                    Name = request.Name, 
                    Email = request.Email, 
                    Password = request.Password,
                    RoleId = request.RoleId
                });
                
            return Ok(userId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
        
    /// <summary>
    /// Endpoint delete user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>
    /// Action result - deleted user guid or
    /// Action result - error message.
    /// </returns>
    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteProject([FromBody] Guid id)
    {
        try
        {
            var userId = await _userService.Delete(id);

            return Ok(userId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }    
    }
}