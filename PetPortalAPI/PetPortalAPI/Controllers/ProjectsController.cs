using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;
using System.Linq;
using System.Reflection;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;

namespace PetPortalAPI.Controllers
{
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
        /// Projects controller constructor.
        /// </summary>
        /// <param name="projectsService">Project service.</param>
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        /// <summary>
        /// Endpoint get paginated projects.
        /// </summary>
        /// <returns>
        /// Action result - List of projects or
        /// Action result - error message.
        /// </returns>
        [HttpGet("{offset:int?}/{page:int?}")]
        public async Task<ActionResult<List<ProjectDetailDto>>> GetProjects(int offset = 10, int page = 1)
        {
            
            // TODO сделать отдельный метод получения проектов, дабы снизить нагрузку на бд. А то все проекты подтаскиваются, а используем только десяток.
            
            if (offset < 1 || page < 1)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync("Error 500. Wrong parameters");
                return BadRequest();
            }

            try
            {
                var projects = await _projectsService.GetAll(); 
                projects = projects
                    .Skip((page - 1) * offset)
                    .Take(offset)
                    .ToList();
                
                var response = projects
                    .Select(p =>
                        new ProjectDetailDto()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            OwnerId = p.OwnerId
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
        /// Endpoint create project.
        /// </summary>
        /// <param name="request">Project detail data.</param>
        /// <returns>
        /// Action result - created project guid or
        /// Action result - error message.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProject([FromBody] ProjectContract request)
        {
            try
            {
                var projectGuid = await _projectsService.Create(request);

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
        /// <param name="request">Project data.</param>
        /// <returns>
        /// Action result - updated project guid or
        /// Action result - error message.
        /// </returns>
        [HttpPut]
        public async Task<ActionResult<Guid>> UpdateProject([FromBody] ProjectDetailDto request)
        {
            try
            {
                var projectId = await _projectsService.Update(request);
                
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
            var token = Request.Headers["Authorization"];

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
