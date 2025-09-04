namespace PetPortalCore.Models;

public class RequiredRole
{
    public Guid RoleId { get; set; }
    
    public string? SystemRoleName { get; set; }
    public string? CustomRoleName { get; set; }
    
    

    public RequiredRole() { } // Для маппинга / сериализации

    public RequiredRole(Guid roleId, string? customRoleName, string? systemRoleName)
    {
        RoleId = roleId;
        CustomRoleName = customRoleName;
        SystemRoleName = systemRoleName;
    }



}