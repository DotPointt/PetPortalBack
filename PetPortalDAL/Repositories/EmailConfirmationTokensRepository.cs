using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Models;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

public class EmailConfirmationTokensRepository : IEmailConfirmationTokensRepository
{
    private readonly PetPortalDbContext _context;

    public EmailConfirmationTokensRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> SaveTokenHash(EmailConfirmationToken token)
    {
        var existing = await _context.EmailConfirmationTokenEntities
            .FirstOrDefaultAsync(t => t.UserId == token.UserId);

        if (existing != null)
        {
            existing.TokenHash = token.TokenHash;
            existing.ExpiresAt = token.ExpiresAt;
            _context.Update(existing);
        }
        else
        {
            await _context.EmailConfirmationTokenEntities.AddAsync(new EmailConfirmationTokenEntity
            {
                Id = token.Id,
                UserId = token.UserId,
                TokenHash = token.TokenHash,
                ExpiresAt = token.ExpiresAt
            });
        }

        await _context.SaveChangesAsync();
        return token.Id;
    }

    public async Task<EmailConfirmationToken> GetTokenHashByUserId(Guid userId)
    {
        var entity = await _context.EmailConfirmationTokenEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (entity == null)
            throw new InvalidOperationException("Токен подтверждения не найден.");

        return EmailConfirmationToken.Create(entity.Id, entity.UserId, entity.TokenHash, entity.ExpiresAt);
    }

    public async Task DeleteByUserIdAsync(Guid userId)
    {
        await _context.EmailConfirmationTokenEntities
            .Where(t => t.UserId == userId)
            .ExecuteDeleteAsync();
    }
}
