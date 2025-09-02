namespace PetPortalCore.Models;

public class RequiredRole
{
    public Guid RoleId { get; set; }
    
    public string? SystemRoleName { get; set; }
    public string? CustomRoleName { get; set; }
    
    

    public RequiredRole() { } // Для маппинга / сериализации

    public RequiredRole(Guid roleId, string? customRoleName)
    {
        RoleId = roleId;
        CustomRoleName = customRoleName;

    }



}