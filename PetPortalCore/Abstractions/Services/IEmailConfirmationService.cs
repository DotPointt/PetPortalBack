using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

public interface IEmailConfirmationService
{
    Task<Guid> SaveTokenHash(EmailConfirmationToken token);
    Task<EmailConfirmationToken> GetTokenHashByUserId(Guid userId);
    string GenerateToken(int byteLength);
    string GenerateConfirmationLink(string baseUrl, string token, Guid userId);
}
