using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs.Contracts;
using PetPortalCore.DTOs.Requests;
using Exception = System.Exception;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Authorization controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    /// <summary>
    /// User service. 
    /// </summary>
    private readonly IUserService _userService;
    
    /// <summary>
    /// Controller constructor. 
    /// </summary>
    /// <param name="userService">User service.</param>
    public AuthorizationController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Endpoint create user.
    /// </summary>
    /// <param name="request">User data.</param>
    /// <returns>
    /// Action result - created user guid or
    /// Action result - error message.
    /// </returns>
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserContract request)
    {
        try
        {
            var userId = await _userService.Register(request);
            var token = await _userService.Login(request.Email, request.Password);
            
            HttpContext.Response.Cookies.Append("jwttoken", token);

            return Ok(new { UserId = userId, Token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Authorization action.
    /// </summary>
    /// <param name="request">Project data.</param>
    /// <returns>
    /// Action result - updated project guid or
    /// Action result - error message.
    /// </returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLoginRequest request)
    {
        try
        {
            var token = await _userService.Login(request.Email, request.Password);

            HttpContext.Response.Cookies.Append("jwttoken", token);

            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}