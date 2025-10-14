using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Контроллер для управления проектами.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    /// <summary>
    /// Сервис для работы с проектами.
    /// </summary>
    private readonly IProjectsService _projectsService;

    /// <summary>
    /// Сервис для работы с пользователями.
    /// </summary>
    private readonly IUserService _usersService;

    /// <summary>
    /// Сервис для работы с объектным хранилищем MinIO.
    /// </summary>
    private readonly IMinioService _minioService;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="projectsService">Сервис для работы с проектами.</param>
    /// <param name="usersService">Сервис для работы с пользователями.</param>
    /// <param name="minioService">Сервис для работы с объектным хранилищем.</param>
    public ProjectsController(IProjectsService projectsService, IUserService usersService, IMinioService minioService)
    {
        _projectsService = projectsService;
        _usersService = usersService;
        _minioService = minioService;
    }

    /// <summary>
    /// Получить список проектов с пагинацией.
    /// </summary>
    /// <param name="request">Запрос на получение проекта.</param>
    /// <returns>
    /// Список проектов.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [SwaggerOperation(Summary = "Стандартный метод получения проектов")]
    [HttpGet()]
    public async Task<ActionResult<List<GetProjectsDto>>> GetProjects([FromQuery] ProjectRequest request)
    //TODO: пока сделал с Base64, но тогда обьем инфы увеличивается на 33%, сделать лучшее отправление, и чтобы ужимались картинки, они оч маленькие
   {
        if (request.Offset < 1 || request.Page < 1)
        {
            Response.StatusCode = 500;
            await Response.WriteAsync("Некорректные параметры запроса.");
            return BadRequest();
        }

        try
        {
            var projects = await _projectsService.GetPaginatedFiltered(request.SortOrder, request.SortItem, request.SearchElement, request.Offset, request.Page, request.Filters);

            var response = new GetProjectsDto();
            var imageBase64 = "";

            foreach (var p in projects)
            {
                var user = await _usersService.GetUserById(p.OwnerId);
                if (!user.AvatarUrl.IsNullOrEmpty())
                {
                    var stream = await _minioService.GetFileAsync(user.AvatarUrl ?? "");
                    var arrayImg = stream.ToArray();
                    imageBase64 = Convert.ToBase64String(arrayImg);
                }

                var projectDto = new ProjectDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,    
                    Requirements = p.Requirements,
                    TeamDescription = p.TeamDescription,
                    Plan = p.Plan,
                    Result = p.Result,
                    OwnerId = p.OwnerId,
                    OwnerName = user.Name,
                    Deadline = p.Deadline,
                    ApplyingDeadline = p.ApplyingDeadline,
                    StateOfProject = p.StateOfProject,
                    AvatarImageBase64 = imageBase64,
                    IsBusinessProject = p.IsBusinesProject,
                    Budget = p.Budget,
                    Tags = p.Tags,
                    RequiredRoles = p.RequiredRoles,
                };

                response.Projects.Add(projectDto);
            }

            response.ProjectsCount = await _projectsService.GetTotalProjectCountAsync(request.SearchElement, request.Filters); //добавить фильтрацию на этот метод

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Получить проект по id.
    /// </summary>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>
    /// Проект.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpGet("{projectId}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById(Guid projectId)
    {
        try
        {
            var project = await _projectsService.GetById(projectId);
            var user = await _usersService.GetUserById(project.OwnerId);
            var imageBase64 = "";

            if (!user.AvatarUrl.IsNullOrEmpty())
            {
                var stream = await _minioService.GetFileAsync(user.AvatarUrl ?? "");
                var arrayImg = stream.ToArray();
                imageBase64 = Convert.ToBase64String(arrayImg);
            }
            
            var projectDto = new ProjectDto()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Requirements = project.Requirements,
                TeamDescription = project.TeamDescription,
                Plan = project.Plan,
                Result = project.Result,
                OwnerId = project.OwnerId,
                OwnerName = user.Name,
                Deadline = project.Deadline,
                ApplyingDeadline = project.ApplyingDeadline,
                StateOfProject = project.StateOfProject,
                AvatarImageBase64 = imageBase64,
                IsBusinessProject = project.IsBusinesProject,
                Budget = project.Budget,
                Tags = project.Tags,
                RequiredRoles = project.RequiredRoles
            };
           
            return Ok(projectDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());

        }
    }

    /// <summary>
    /// Создать новый проект.
    /// </summary>
    /// <param name="projectRequest">Данные для создания проекта.</param>
    /// <returns>
    /// Идентификатор созданного проекта.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> CreateProject([FromBody] ProjectContract projectRequest) //сделать отдельный класс? в общем не должно быть неразберихи
    {
        try
        {
            var userid = await _usersService.GetUserIdFromJWTAsync(User);
        
            var valid = await _projectsService.CheckCreatingLimit(userid!.Value, limit: 100);
            if (!valid)
                return BadRequest("Вы превысили лимит проектов.");


            projectRequest.StateOfProject = StateOfProject.Open;

            var projectGuid = await _projectsService.Create(projectRequest, userid.Value);

            return Ok(projectGuid);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Обновить данные проекта.
    /// </summary>
    /// <param name="id">Идентификатор проекта.</param>
    /// <param name="request">Данные для обновления проекта.</param>
    /// <returns>
    /// Идентификатор обновленного проекта.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateProject(Guid id, [FromBody] ProjectDto request)
    {
        try
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("Идентификатор пользователя не найден в токене.");
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
                return Forbid("Вы не являетесь владельцем этого проекта.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Удалить проект.
    /// </summary>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>
    /// Идентификатор удаленного проекта.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpDelete]
    public async Task<ActionResult<Guid>> DeleteProject([FromBody] Guid id)
    {
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