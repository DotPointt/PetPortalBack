using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Repositories;

public interface IEmailConfirmationTokensRepository
{
    Task<Guid> SaveTokenHash(EmailConfirmationToken token);
    Task<EmailConfirmationToken> GetTokenHashByUserId(Guid userId);
    Task DeleteByUserIdAsync(Guid userId);
}
