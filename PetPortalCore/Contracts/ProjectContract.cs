using PetPortalCore.Models.ProjectModels;

namespace PetPortalCore.DTOs.Contracts;

/// <summary>
/// Project create contract.
/// </summary>
public class ProjectContract
{
    /// <summary>
    /// Project name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Project description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Project owner.
    /// </summary>
    public Guid OwnerId { get; set; }
    
    /// <summary>
    /// Time when owner thinks project should be completed. Infinite when null
    /// </summary>
    public DateTime? Deadline  { get; set; }

    /// <summary>
    /// Interval of time left for joining project. Infinite when null( now interval will be calculated at frontend, but we can send TimeSpan or string)?
    /// </summary>
    public DateTime? ApplyingDeadline { get; set; } = null;

    /// <summary>
    /// If people can join the project at the moment
    /// </summary>
    /// 
    public StateOfProject StateOfProject = StateOfProject.Closed;
}