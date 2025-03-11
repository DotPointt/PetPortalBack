using PetPortalCore.Models.ProjectModels;

namespace PetPortalCore.DTOs;

/// <summary>
/// Project dto.
/// </summary>
public class ProjectDto
{
    /// <summary>
    /// Project identifier.
    /// </summary>
    public Guid Id { get; set; }
        
    /// <summary>
    /// Project name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Project description.
    /// </summary>
    public string Description { get; set; }
        
    /// <summary>
    /// Project owner identifier.
    /// </summary>
    public Guid OwnerId { get; set; }
    
    /// <summary>
    /// Time when owner thinks project should be completed. Infinite when null
    /// </summary>
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Interval of time left for joining project. Infinite when null( now interval will be calculated at frontend, but we can send TimeSpan or string)?
    /// </summary>
    public DateTime? ApplyingDeadline { get; set; } = null;

    /// <summary>
    /// Price of the project in rubles
    /// </summary>
    public uint Budget { get; set; } = 0;

    /// <summary>
    /// If people can join the project at the moment
    /// </summary>
    public StateOfProject StateOfProject { get; set; } = StateOfProject.Closed;
    
    /// <summary>
    /// If project is to be done for money
    /// </summary>
    public bool IsBusinesProject = false;
    
    public string AvatarImageBase64 { get; set; }
}