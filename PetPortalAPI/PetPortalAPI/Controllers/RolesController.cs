using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;

namespace PetPortalAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRolesService _rolesService;


    public RolesController(IRolesService rolesService)
    {
        _rolesService = rolesService;
    }
    
    [HttpGet("AllRoles")]
    public async Task<ActionResult<List<Role>>> Get()
    {
        try
        {
            var responds = await _rolesService.GetAll();
            
            return Ok(responds);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}