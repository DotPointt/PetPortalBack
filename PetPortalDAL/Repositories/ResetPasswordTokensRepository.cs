using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Models;
using PetPortalDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalDAL.Repositories
{
    public class ResetPasswordTokensRepository : IResetPasswordTokensRepository
    {
        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly PetPortalDbContext _context;

        public ResetPasswordTokensRepository(PetPortalDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> SaveTokenHash(ResetPasswordTokens token)
        {
            var existingToken = await _context.ResetPasswordTokens
                    .FirstOrDefaultAsync(t => t.UserId == token.UserId);

            if (existingToken != null)
            {
                // Если токен существует, обновляем его данные
                existingToken.TokenHash = token.TokenHash;
                existingToken.ExpiresAt = token.ExpiresAt;
                _context.Update(existingToken); // Маркируем запись как изменённую
            }
            else
            {
                // Если токена нет, добавляем новый
                await _context.AddAsync(token);
            }

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();

            return token.Id;
        }

        public async Task<ResetPasswordTokens> GetTokenHashByUserId(Guid userId)
        {
            var tokenEntity = await _context.ResetPasswordTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(tokenEntity => tokenEntity.UserId == userId);

            if (tokenEntity == null)
                throw new Exception("Токен не найден.");

            return ResetPasswordTokens.Create(
                    tokenEntity.Id,
                    tokenEntity.UserId,
                    tokenEntity.TokenHash,
                    tokenEntity.ExpiresAt
                );
        }
    }
}
