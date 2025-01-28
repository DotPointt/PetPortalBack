using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs.Requests;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Authorization controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IUserService _userService;
    
    public AuthorizationController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Authroization action.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <param name="request">Project data.</param>
    /// <returns>
    /// Action result - updated project guid or
    /// Action result - error message.
    /// </returns>
    [HttpPost()]
    public async Task<IResult> Login([FromBody] UserLoginRequest request)
    {
        try
        {
            string token = await _userService.Login(request.Email, request.Password);

            HttpContext.Response.Cookies.Append("jwttoken", token);
            
            return Results.Ok();
        }
        catch
        {
            return Results.Unauthorized();
        }
    }
}