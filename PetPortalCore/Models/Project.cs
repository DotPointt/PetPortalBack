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

    public Project()
    {
        
    }
    
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

    private Project(Guid id, string name, string description,string requirements, 
        string teamDescription, 
        string plan, 
        string result,  Guid ownerId, DateTime? deadline, DateTime? applyingDeadline, StateOfProject stateOfProject)
    {
        Id = id;
        Name = name;
        Description = description;
        Requirements = requirements;
        TeamDescription = teamDescription;
        Plan = plan;
        Result = result;
        OwnerId = ownerId;
        Deadline = deadline;
        ApplyingDeadline = applyingDeadline;
        StateOfProject = stateOfProject;
    }
    

    /// <summary>
    /// Project identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Project name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Project description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Description of requirements needed to be satisfied to complete the projects
    /// </summary>
    public string Requirements { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of current and future team members
    /// </summary>
    public string TeamDescription { get; set; } = string.Empty;

    /// <summary>
    /// Plan of project
    /// </summary>
    public string Plan { get; set; } = string.Empty;
        
    /// <summary>
    /// Description of awaited result of project
    /// </summary>
    public string Result { get; set; } = string.Empty;

    /// <summary>
    /// Project owner.
    /// </summary>
    public Guid OwnerId { get; set; } = Guid.Empty;

    /// <summary>
    /// Time when owner thinks project should be completed. Infinite when null
    /// </summary>
    public DateTime? Deadline { get; set; }  = null;

    /// <summary>
    /// Interval of time left for joining project. Infinite when null( now interval will be calculated at frontend, but we can send TimeSpan or string)?
    /// </summary>
    public DateTime? ApplyingDeadline { get; set; } = null;

    /// <summary>
    /// If people can join the project at the moment
    /// </summary>
    public StateOfProject StateOfProject { get; set; } = StateOfProject.Closed;

    /// <summary>
    /// If project is to be done for money
    /// </summary>
    public bool IsBusinesProject { get; set; } = false;
    
    /// <summary>
    /// Price of the project in rubles
    /// </summary>
    public uint Budget { get; set; }
    
    /// связка с фреймворками
    /// <summary>
    /// Список тегов, связанных с проектом.
    /// </summary>
    public List<Tag> Tags { get; set; }

    /// <summary> 
    /// Creation new project.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <param name="name">Project name.</param>
    /// <param name="description">Project description.</param>
    /// <param name="ownerId">Project owner identifier.</param>
    /// <returns>(project, error if it exist)</returns>

    public static (Project project, string Error) Create(Guid id, string name, string description,   string requirements, 
        string teamDescription, 
        string plan, 
        string result,Guid ownerId,bool IsBusinesProject, uint Budget,  DateTime? Deadline = null, DateTime? ApplyingDeadline = null, StateOfProject StateOfProject = StateOfProject.Closed
        )
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGHT)
        {
            error = "Name can not be empty or longer then 250 symbols";
        }

        var project = new Project(id, name, description, requirements, 
            teamDescription, 
            plan, 
            result, ownerId, Deadline, ApplyingDeadline, StateOfProject);

        return (project, error);
    }

    //public abstract (Project project, string Error) Create(Guid id, string name, string description, Guid ownerId, DateTime? Deadline = null, DateTime? ApplyingDeadline = null, StateOfProject StateOfProject = StateOfProject.Closed);

}


public enum StateOfProject
{
    Open = 0,
    InProgress = 1,
    Closed = 2
}