using PetPortalCore.Models;

namespace PetPortalCore.DTOs;

public class ProjectFilterDTO
{
    public Guid? RoleId { get; set; }
    public string? Deadline { get; set;  }
    public bool? IsCommercial { get; set; }

    public StateOfProject StateOfProject { get; set; }
    
    public List<Guid>? Tags { get; set; }
}