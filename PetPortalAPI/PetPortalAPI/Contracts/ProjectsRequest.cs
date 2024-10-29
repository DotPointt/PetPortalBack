namespace PetPortalAPI.Contracts
{
    /// <summary>
    /// Project data.
    /// </summary>
    /// <param name="Name">Project name.</param>
    /// <param name="Description">Project description.</param>
    public record ProjectsRequest
    (
        string Name,
        string Description
    );
}
