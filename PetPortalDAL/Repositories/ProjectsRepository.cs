using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalCore.Models;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories
{
    /// <summary>
    /// Project repository.
    /// </summary>
    public class ProjectsRepository : IProjectsRepository
    {
        /// <summary>
        /// Data base context.
        /// </summary>
        private readonly PetPortalDbContext _context;
        
        /// <summary>
        /// Repository constructor.
        /// </summary>
        /// <param name="context">Data base context.</param>
        public ProjectsRepository(PetPortalDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get data bases projects.
        /// </summary>
        /// <returns>List of projects.</returns>
        public async Task<List<Project>> Get()
        {
            var projectsEntities = await _context.Projects
                .AsNoTracking()
                .ToListAsync();

            var projects = projectsEntities
                .Select(project =>
                    Project.Create(project.Id, project.Name, project.Description, project.OwnerId).project)
                .ToList();

            return projects;
        }

        /// <summary>
        /// Create new project in data base.
        /// </summary>
        /// <param name="projectData">Project data.</param>
        /// <returns>Created project identifier.</returns>
        public async Task<Guid> Create(Project projectData)
        {
            var projectEntity = new ProjectEntity()
            {
                Id = projectData.Id,
                Name = projectData.Name,
                Description = projectData.Description,
                OwnerId = projectData.OwnerId
            };

            await _context.AddAsync(projectEntity);
            await _context.SaveChangesAsync();

            return projectEntity.Id;
        }

        /// <summary>
        /// Update data base project.
        /// </summary>
        /// <param name="projectData">Project data.</param>
        /// <returns>Updated project identifier.</returns>
        public async Task<Guid> Update(ProjectDetailDto projectData)
        {
            await _context.Projects
                .Where(project => project.Id == projectData.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(project => project.Name, project => projectData.Name)
                    .SetProperty(project => project.Description, project => projectData.Description)
                );

            return projectData.Id;
        }

        /// <summary>
        /// Delete data base project.
        /// </summary>
        /// <param name="id">Project identifier.</param>
        /// <returns>Deleted project identifier.</returns>
        public async Task<Guid> Delete(Guid id)
        {
            await _context.Projects
                .Where(project => project.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}