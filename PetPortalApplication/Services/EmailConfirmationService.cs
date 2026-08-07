using System.Security.Cryptography;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

public class EmailConfirmationService : IEmailConfirmationService
{
    private readonly IEmailConfirmationTokensRepository _tokensRepository;

    public EmailConfirmationService(IEmailConfirmationTokensRepository tokensRepository)
    {
        _tokensRepository = tokensRepository;
    }

    public Task<Guid> SaveTokenHash(EmailConfirmationToken token) =>
        _tokensRepository.SaveTokenHash(token);

    public Task<EmailConfirmationToken> GetTokenHashByUserId(Guid userId) =>
        _tokensRepository.GetTokenHashByUserId(userId);

    public string GenerateToken(int byteLength)
    {
        var randomBytes = new byte[byteLength];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
    }

    public string GenerateConfirmationLink(string baseUrl, string token, Guid userId) =>
        $"{baseUrl}?token={token}&userId={userId}";
}
