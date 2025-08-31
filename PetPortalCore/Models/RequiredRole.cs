namespace PetPortalCore.Models;

public class RequiredRole
{
    public Guid RoleId { get; }
    public string? CustomRoleName { get; }
    

    private RequiredRole() { } // Для маппинга / сериализации

    public RequiredRole(Guid roleId, string? customRoleName)
    {
        RoleId = roleId;
        CustomRoleName = customRoleName;

    }



}