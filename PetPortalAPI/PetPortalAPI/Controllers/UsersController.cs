using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MapsterMapper;
using PetPortalCore.DTOs.Contracts;
using Microsoft.AspNetCore.Authorization;
using PetPortalApplication.Services;
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

    /// <summary>
    /// Auth provider.
    /// </summary>
    private readonly IJwtProvider _jwtProvider;

    /// <summary>
    /// MinIO service.
    /// </summary>
    private readonly IMinioService _minioService;
    
    /// <summary>
    /// Users controller constructor.
    /// </summary> 
    /// <param name="minioService">MinIO service.</param>
    /// <param name="userService">Users service.</param>
    /// <param name="jwtProvider">Auth provider.</param>
    public UsersController(IUserService userService, IJwtProvider jwtProvider, IMinioService minioService)
    {
        _userService = userService;
        _jwtProvider = jwtProvider;
        _minioService = minioService;
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
                        Password = p.PasswordHash,
                        AvatarUrl = p.AvatarUrl,
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
    /// <param name="request">User updated data.</param>
    /// <returns>
    /// Action result - updated user guid or
    /// Action result - error message.
    /// </returns>
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UserDto request)
    {
        try
        {
            var Id = await _userService.Update(request);
            
            return Ok(Id);
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
    public async Task<ActionResult<Guid>> DeleteUser([FromBody] Guid id)
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