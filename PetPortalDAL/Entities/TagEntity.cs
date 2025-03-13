using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Entities;

/// <summary>
/// Сущность фреймворка в базе данных.
/// </summary>
public class TagEntity
{
    /// <summary>
    /// Идентификатор фреймворка.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название фреймворка.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Список связей между фреймворком и проектами.
    /// </summary>
    public ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
}