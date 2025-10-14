using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

public interface IRolesService
{
    Task<ICollection<Role>> GetAll();
}