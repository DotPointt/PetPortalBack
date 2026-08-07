namespace PetPortalCore.DTOs;

public class ProjectFilterDTO
{
    public Guid? RoleId { get; set; }
    public string? Deadline { get; set;  }
    public bool? IsCommercial { get; set; }

    /// <summary>
    /// Показывать ли архивные проекты. По умолчанию каталог отдаёт только те,
    /// по которым идёт набор.
    /// </summary>
    public bool ShowArchived { get; set; } = false;

    public List<Guid>? Tags { get; set; }
}
