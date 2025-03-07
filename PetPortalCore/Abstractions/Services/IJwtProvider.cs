using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

public interface IJwtProvider
{
    public string GenerateToken(Guid userId, string email, string roleName);
}