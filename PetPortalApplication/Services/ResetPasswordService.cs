using System.Security.Cryptography;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

public class ResetPasswordService : IResetPasswordService
{
    private readonly IResetPasswordTokensRepository _TokensRepository;

    public ResetPasswordService(IResetPasswordTokensRepository tokensRepository)
    {
        _TokensRepository = tokensRepository;
    }

    public async Task<Guid> SaveTokenHash(ResetPasswordTokens token)
    {
        return await _TokensRepository.SaveTokenHash(token);
    }

    public async Task<ResetPasswordTokens> GetTokenHashByUserId(Guid userId)
    {
        return await _TokensRepository.GetTokenHashByUserId(userId);
    }
    
    /// <summary>
    /// Генерирует токен(строку) в hex формате, для восстановлеиня пароля
    /// </summary>
    /// <param name="byteLength"></param>
    /// <returns></returns>
    public string GenerateResetPasswordToken(int byteLength)
    {
        byte[] randomBytes = new byte[byteLength];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes); // Заполняем массив случайными байтами
        }
        return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
    }
    
    public string GeneratePasswordResetLink(string baseUrl, string token, Guid userId)
    {
        // Формирование ссылки
        string resetLink = $"{baseUrl}?token={token}&userId={userId}";

        return resetLink;
    }
}