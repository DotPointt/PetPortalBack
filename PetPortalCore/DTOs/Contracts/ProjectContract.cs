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
}