namespace PetPortalDAL.Entities;

public class EmailConfirmationTokenEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserEntity User { get; set; } = null!;
}
