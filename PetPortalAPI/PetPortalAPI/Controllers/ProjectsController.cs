using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;
using PetPortalCore.DTOs.Requests;
using PetPortalCore.Models.ProjectModels;
using Swashbuckle.AspNetCore.Annotations;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Projects controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    /// <summary>
    /// Projects service.
    /// </summary>
    private readonly IProjectsService _projectsService;

    /// <summary>
    /// UsersService
    /// </summary>
    private readonly IUserService _usersService;

    private readonly IMinioService _minioService;

    /// <summary>
    /// Projects controller constructor.
    /// </summary>
    /// <param name="projectsService">Project service.</param>
    /// <param name="usersService">Users service.</param>
    /// <param name="minioService">Object storage service.</param>
    public ProjectsController(IProjectsService projectsService, IUserService usersService, IMinioService minioService)
    {
        _projectsService = projectsService;
        _usersService = usersService;
        _minioService = minioService;
    }

    /// <summary>
    /// Endpoint get paginated projects.
    /// </summary>
    /// <returns>
    /// Action result - List of projects or
    /// Action result - error message.
    /// </returns>
    [SwaggerOperation(Summary = "Стандартный метод получения проектов")]
    [HttpGet()]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects([FromQuery] ProjectRequest request) //TODO: пока сделал с Base64, но тогда обьем инфы увеличивается на 33%, сделать лучшее отправление, и чтобы ужимались картинки, они оч маленькие
    {
        if (request.offset < 1 || request.page < 1)
        {
            Response.StatusCode = 500;
            await Response.WriteAsync("Wrong parameters");
            return BadRequest();
        }

        try
        {
            var projects = await _projectsService.GetPaginatedFiltered(request.SortItem, request.SortOrder, request.offset, request.page);

            if (projects.Count == 0)
            {
                return Ok("There are no projects here yet.");
            }

            List<ProjectDto> response = new List<ProjectDto>();
            
            
            foreach (var p in projects)
            {
                var user = await _usersService.GetUserById(p.OwnerId);
                var stream = await _minioService.GetFileAsync(user.AvatarUrl ?? "");
                byte[] arrayimg = stream.ToArray();
                var imageBase64 =  Convert.ToBase64String(arrayimg);
                
                
                var projectDto = new ProjectDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    OwnerId = p.OwnerId,
                    OwnerName = user.Name,
                    Deadline = p.Deadline,
                    ApplyingDeadline = p.ApplyingDeadline,
                    StateOfProject = p.StateOfProject,
                    AvatarImageBase64 = imageBase64,
                    IsBusinesProject = p.IsBusinesProject,
                    Budget = p.Budget,
                    Tags = new List<string>()
                };
                
                response.Add(projectDto);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Endpoint create project.
    /// </summary>
    /// <param name="projectRequest">Project detail data.</param>
    /// <returns>
    /// Action result - created project guid or
    /// Action result - error message.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProject([FromBody] ProjectContract projectRequest) //сделать отдельный класс? в общем не должно быть неразберихи
    {
        bool valRes = await _projectsService.CheckCreatingLimit(projectRequest.OwnerId, limit : 100);
        // if (valRes)
            // return BadRequest("Вы превысили лимит проектов.");

        try
        {
            projectRequest.StateOfProject = StateOfProject.Open;

            var projectGuid = await _projectsService.Create(projectRequest);
            
            return Ok(projectGuid);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Endpoint update project.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <param name="request">Project data.</param>
    /// <returns>
    /// Action result - updated project guid or
    /// Action result - error message.
    /// </returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateProject(Guid id, [FromBody] ProjectDto request)
    {
        try
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in claims.");
            }
            
            var project = await _projectsService.GetById(id);
            var userId = Guid.Parse(userIdClaim);

            if (project.OwnerId == userId)
            {
                var projectId = await _projectsService.Update(request);

                return Ok(projectId);
            }
            else
            {
                return Forbid("You are not the owner of this project.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Endpoint delete project.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <returns>
    /// Action result - deleted project guid or
    /// Action result - error message.
    /// </returns>
    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteProject([FromBody] Guid id)
    {
        // var token = Request.Headers["Authorization"];
        
        try
        {
            var projectId = await _projectsService.Delete(id);

            return Ok(projectId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }    
    }   
}