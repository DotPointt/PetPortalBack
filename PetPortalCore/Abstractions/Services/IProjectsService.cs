using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services
{
    /// <summary>
    /// Project service interface.
    /// </summary>
    public interface IProjectsService
    {
        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <returns>List of projects.</returns>
        Task<List<Project>> GetAllProjects();
        
        /// <summary>
        /// Create new project.
        /// </summary>
        /// <param name="project">Project data.</param>
        /// <returns>Identifier of new project.</returns>
        Task<Guid> CreateProject(Project project);
        
        /// <summary>
        /// Project update.
        /// </summary>
        /// <param name="id">Project identifier.</param>
        /// <param name="name">Project name.</param>
        /// <param name="description">Project description.</param>
        /// <returns>Identifier of updated project.</returns>
        Task<Guid> UpdateProject(Guid id, string name, string description);
        
        /// <summary>
        /// Delete project.
        /// </summary>
        /// <param name="id">Project identifier.</param>
        /// <returns>Identifier of deleted project.</returns>
        Task<Guid> Delete(Guid id);
    }
}
