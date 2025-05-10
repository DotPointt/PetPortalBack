using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

public interface IResetPasswordService
{
    public Task<Guid> SaveTokenHash(ResetPasswordTokens token);
    public Task<ResetPasswordTokens> GetTokenHashByUserId(Guid userId);
    public string GenerateResetPasswordToken(int byteLength);

    public string GeneratePasswordResetLink(string baseUrl, string token, Guid userId);
}