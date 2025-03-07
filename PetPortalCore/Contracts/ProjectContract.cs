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
    /// Name of the owner
    /// </summary>
    public string OwnerName { get; set; }
    
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

    /// <summary>
    /// Price of the project in rubles
    /// </summary>
    public uint Budget;

    /// <summary>
    /// If project is to be done for money
    /// </summary>
    public bool IsBusinesProject = false;
    
    /// <summary>
    /// Tags related to Project. List of strings.
    /// </summary>
    public List<string> Tags { get; set; }

    /// <summary>
    /// Avatar img in Base64 encoding
    /// </summary>
    public string AvatarImageBase64 { get; set; }
}