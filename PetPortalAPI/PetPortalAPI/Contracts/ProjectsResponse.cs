namespace PetPortalAPI.Contracts
{
    /// <summary>
    /// Project data.
    /// </summary>
    /// <param name="Id">Project identifier.</param>
    /// <param name="Name">Project name.</param>
    /// <param name="Description">Project description</param>
    public record ProjectsResponse
    (
        Guid Id,
        string Name,
        string Description
    );
}
