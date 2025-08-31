using System.ComponentModel.DataAnnotations;
using PetPortalCore.Models;

namespace PetPortalDAL.Entities.LinkingTables;

public class ProjectRole
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid ProjectId { get; set; }
    public ProjectEntity Project { get; set; } = null!;
    
    
    public Guid RoleId { get; set; }
    
    public RoleEntity Role { get; set; } = null!;


    public string? CustomRoleName { get; set; } = null;
}