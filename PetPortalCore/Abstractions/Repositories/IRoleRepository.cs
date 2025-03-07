namespace PetPortalDAL.Repositories;

public interface IRoleRepository
{
    /// <summary>
    /// get user role name.
    /// </summary>
    /// <param name="userId">user id.</param>
    /// <returns>Role name.</returns>
    Task<string> GetRoleByUserId(Guid userId);
}