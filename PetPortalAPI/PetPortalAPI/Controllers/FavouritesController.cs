using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;
using PetPortalDAL;
using PetPortalDAL.Entities;

namespace PetPortalAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FavouritesController : ControllerBase
{
    private readonly PetPortalDbContext _db;
    private readonly IUserService _userService;
    private readonly IProjectsService _projectsService;

    public FavouritesController(
        PetPortalDbContext db,
        IUserService userService,
        IProjectsService projectsService)
    {
        _db = db;
        _userService = userService;
        _projectsService = projectsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Project>>> GetMyFavourites()
    {
        var userId = await _userService.GetUserIdFromJWTAsync(User);
        if (userId == null)
            return Unauthorized();

        var projectIds = await _db.Favourites
            .AsNoTracking()
            .Where(f => f.UserId == userId.Value)
            .Select(f => f.ProjectId)
            .ToListAsync();

        var projects = new List<Project>();
        foreach (var id in projectIds)
        {
            try
            {
                projects.Add(await _projectsService.GetById(id));
            }
            catch
            {
                // skip missing
            }
        }

        return Ok(projects);
    }

    [HttpGet("contains/{projectId:guid}")]
    public async Task<ActionResult<bool>> IsFavourite(Guid projectId)
    {
        var userId = await _userService.GetUserIdFromJWTAsync(User);
        if (userId == null)
            return Unauthorized();

        var exists = await _db.Favourites
            .AnyAsync(f => f.UserId == userId.Value && f.ProjectId == projectId);
        return Ok(exists);
    }

    [HttpPost("{projectId:guid}")]
    public async Task<ActionResult> Add(Guid projectId)
    {
        var userId = await _userService.GetUserIdFromJWTAsync(User);
        if (userId == null)
            return Unauthorized();

        var exists = await _db.Favourites
            .AnyAsync(f => f.UserId == userId.Value && f.ProjectId == projectId);
        if (exists)
            return Ok();

        _db.Favourites.Add(new FavouriteEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId.Value,
            ProjectId = projectId,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{projectId:guid}")]
    public async Task<ActionResult> Remove(Guid projectId)
    {
        var userId = await _userService.GetUserIdFromJWTAsync(User);
        if (userId == null)
            return Unauthorized();

        var fav = await _db.Favourites
            .FirstOrDefaultAsync(f => f.UserId == userId.Value && f.ProjectId == projectId);
        if (fav != null)
        {
            _db.Favourites.Remove(fav);
            await _db.SaveChangesAsync();
        }

        return Ok();
    }
}
