namespace PetPortalCore.Models;

/// <summary>
/// Project.
/// </summary>
public class Project
{
    /// <summary>
    /// Max name length.
    /// </summary>
    private const int MAX_NAME_LENGHT = 250;

    /// <summary>
    /// Project constructor.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <param name="name">Project name.</param>
    /// <param name="description">Project description.</param>
    /// <param name="ownerId">Project owner identifier.</param>
    private Project(Guid id, string name, string description, Guid ownerId)
    {
        Id = id;
        Name = name;
        Description = description;
        OwnerId = ownerId;
    }
    
    private Project(Guid id, string name, string description, Guid ownerId, DateTime? deadline, DateTime? applyingDeadline, bool isOpen)
    {
        Id = id;
        Name = name;
        Description = description;
        OwnerId = ownerId;
        Deadline = deadline;
        ApplyingDeadline = applyingDeadline;
        IsOpen = isOpen;
    }

    /// <summary>
    /// Project identifier.
    /// </summary>
    public Guid Id;

    /// <summary>
    /// Project name.
    /// </summary>
    public string Name = string.Empty;

    /// <summary>
    /// Project description.
    /// </summary>
    public string Description = string.Empty;

    /// <summary>
    /// Project owner.
    /// </summary>
    public Guid OwnerId = Guid.Empty;

    /// <summary>
    /// Time when owner thinks project should be completed. Infinite when null
    /// </summary>
    public DateTime? Deadline = null;

    /// <summary>
    /// Interval of time left for joining project. Infinite when null( now interval will be calculated at frontend, but we can send TimeSpan or string)?
    /// </summary>
    public DateTime? ApplyingDeadline = null;

    /// <summary>
    /// If people can join the project at the moment
    /// </summary>
    public bool IsOpen = false;
    
    /// <summary>
    /// Creation new project.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <param name="name">Project name.</param>
    /// <param name="description">Project description.</param>
    /// <param name="ownerId">Project owner identifier.</param>
    /// <returns>(project, error if it exist)</returns>
    public static (Project project, string Error) Create(Guid id, string name, string description, Guid ownerId, DateTime? Deadline = null, DateTime? ApplyingDeadline = null, bool IsOpen = false)
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGHT)
        {
            error = "Name can not be empty or longer then 250 symbols";
        }

        var project = new Project(id, name, description, ownerId, Deadline, ApplyingDeadline, IsOpen);

        return (project, error);
    }
}