using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Repositories
{
    /// <summary>
    /// Project repository interface.
    /// </summary>
    public interface IProjectsRepository
    {
        /// <summary>
        /// Get projects from db.
        /// </summary>
        /// <returns>List of projects.</returns>
        Task<List<Project>> Get();
        
        /// <summary>
        /// Create new project in db.
        /// </summary>
        /// <param name="project">Project data.</param>
        /// <returns>Project identifier.</returns>
        Task<Guid> Create(Project project);
        
        /// <summary>
        /// Update project in db.
        /// </summary>
        /// <param name="id">Project identifier.</param>
        /// <param name="name">Project name.</param>
        /// <param name="description">Project description.</param>
        /// <returns>Identifier of updated project.</returns>
        Task<Guid> Update(Guid id, string name, string description);
        
        /// <summary>
        /// Delete project in db.
        /// </summary>
        /// <param name="id">Project identifier.</param>
        /// <returns>Identifier of deleted project.</returns>
        Task<Guid> Delete(Guid id);
    }
}
