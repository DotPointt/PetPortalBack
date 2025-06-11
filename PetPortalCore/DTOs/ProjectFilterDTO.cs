using PetPortalCore.Models;

namespace PetPortalCore.DTOs;

public class ProjectFilterDTO
{
    public string? Role { get; set; }
    public string? Deadline { get; set;  }
    public bool? IsCommercial { get; set; }
    public List<Tag>? Tags { get; set; }
}