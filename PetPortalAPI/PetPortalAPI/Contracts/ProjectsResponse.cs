namespace PetPortalAPI.Contracts
{
    public record ProjectsResponse(
        Guid Id,
        string name,
        string description);
}
