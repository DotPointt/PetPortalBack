using Microsoft.AspNetCore.Mvc;
using PetPortalAPI.Contracts;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;
using System.Linq;
using System.Reflection;

namespace PetPortalAPI.Controllers
{
    /// <summary>
    /// Project controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        /// <summary>
        /// Project service.
        /// </summary>
        private readonly IProjectsService _projectsService;

        /// <summary>
        /// Project controller constructor.
        /// </summary>
        /// <param name="projectsService">Project service.</param>
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        /// <summary>
        /// Endpoint get all projects.
        /// </summary>
        /// <returns>Action result - List of projects.</returns>
        [HttpGet("{offset:int?}/{page:int?}")]
        public async Task<ActionResult<List<ProjectsResponse>>> GetProjects(int offset = 10, int page = 1)
        {
            if (offset < 1 || page < 1)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync("Error 500. DivideByZeroException occurred!");
                return BadRequest();
            }

            try
            {
                var projects = await _projectsService.GetAllProjects(); projects = projects.Skip((page - 1) * offset).Take(offset).ToList();
                var response = projects.Select(p => new ProjectsResponse(p.Id, p.Name, p.Description));

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
        /// <param name="request">Project data.</param>
        /// <returns>
        /// Action result - created project guid or
        /// Action result - error message.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProject([FromBody] ProjectsRequest request)
        {
            try
            {
                var (project, error) = Project.Create(
                    Guid.NewGuid(),
                    request.Name,
                    request.Description);
                
                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }
                
                var projectGuid = await _projectsService.CreateProject(project);

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
        public async Task<ActionResult<Guid>> UpdateProject(Guid id, [FromBody] ProjectsRequest request)
        {
            try
            {
                var projectId = await _projectsService.UpdateProject(id, request.Name, request.Description);
                
                return Ok(projectId);
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
}
