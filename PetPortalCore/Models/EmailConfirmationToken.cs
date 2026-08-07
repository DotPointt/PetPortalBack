namespace PetPortalCore.Models;

public class EmailConfirmationToken
{
    private EmailConfirmationToken(Guid id, Guid userId, string tokenHash, DateTime expiresAt)
    {
        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
    }

    public static EmailConfirmationToken Create(Guid id, Guid userId, string tokenHash, DateTime expiresAt)
        => new(id, userId, tokenHash, expiresAt);

    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
