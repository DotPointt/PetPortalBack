namespace PetPortalDAL.Entities;

/// <summary>
/// Избранный проект пользователя.
/// </summary>
public class FavouriteEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public UserEntity? User { get; set; }
    public ProjectEntity? Project { get; set; }
}
