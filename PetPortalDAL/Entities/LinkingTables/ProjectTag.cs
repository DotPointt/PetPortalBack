namespace PetPortalDAL.Entities.LinkingTables;

/// <summary>
/// Связующая сущность между проектами и тегами.
/// </summary>
public class ProjectTag
{
    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Идентификатор тега.
    /// </summary>
    public Guid TagId { get; set; }

    /// <summary>
    /// Ссылка на сущность тега.
    /// </summary>
    public TagEntity Tag { get; set; }

    /// <summary>
    /// Ссылка на сущность проекта.
    /// </summary>     
    public ProjectEntity Project { get; set; }
}