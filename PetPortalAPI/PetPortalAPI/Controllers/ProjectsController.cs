using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
using PetPortalCore.Models;

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
    /// <param name="offset">Количество проектов на странице.</param>
    /// <param name="page">Номер страницы.</param>
    /// <returns>
    /// Список проектов.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpGet("{offset:int?}/{page:int?}")]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects(int offset = 10, int page = 1)
    {
        if (offset < 1 || page < 1)
        {
            Response.StatusCode = 500;
            await Response.WriteAsync("Некорректные параметры запроса.");
            return BadRequest();
        }

        try
        {
            var projects = await _projectsService.GetPaginated(offset, page);

            if (projects.Count == 0)
            {
                return Ok("Пока что проектов нет.");
            }

            var response = new List<ProjectDto>();
            var imageBase64 = "";

            foreach (var p in projects)
            {
                var user = await _usersService.GetUserById(p.OwnerId);
                if (!user.AvatarUrl.IsNullOrEmpty())
                {
                    var stream = await _minioService.GetFileAsync(user.AvatarUrl ?? "DefaultBucketKey");
                    var arrayImg = stream.ToArray();
                    imageBase64 = Convert.ToBase64String(arrayImg);
                }

                var projectDto = new ProjectDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    OwnerId = p.OwnerId,
                    Deadline = p.Deadline,
                    ApplyingDeadline = p.ApplyingDeadline,
                    StateOfProject = p.StateOfProject,
                    AvatarImageBase64 = imageBase64,
                    IsBusinessProject = p.IsBusinesProject,
                    Budget = p.Budget
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
    /// Создать новый проект.
    /// </summary>
    /// <param name="projectRequest">Данные для создания проекта.</param>
    /// <returns>
    /// Идентификатор созданного проекта.
    /// В случае ошибки возвращает сообщение об ошибке.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProject([FromBody] ProjectContract projectRequest)
    {
        var valid = await _projectsService.CheckCreatingLimit(projectRequest.OwnerId, limit: 100);
        if (!valid)
            return BadRequest("Вы превысили лимит проектов.");

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