using PetPortalCore.Models.ProjectModels;
using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Entities;

/// <summary>
/// Project as data base model.
/// </summary>
public class ProjectEntity : BaseAuditableEntity
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
    /// Project owner.
    /// </summary>
    public Guid OwnerId { get; set; }
        
    /// <summary>
    /// Owner.
    /// </summary>
    public UserEntity Owner { get; set; }
    
    /// <summary>
    /// Time when owner thinks project should be completed. Infinite when null
    /// </summary>
    public DateTime? Deadline { get; set; } = null;

    /// <summary>
    /// Interval of time left for joining project. Infinite when null( now interval will be calculated at frontend, but we can send TimeSpan or string)?
    /// </summary>
    public DateTime? ApplyingDeadline { get; set; } = null;


    /// <summary>
    /// If people can join the project at the moment
    /// </summary>
    public StateOfProject StateOfProject = StateOfProject.Closed;

    /// <summary>
    /// Price of the project in rubles
    /// </summary>
    public uint Budget;

    public ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
}