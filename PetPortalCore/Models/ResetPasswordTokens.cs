using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalCore.Models
{
    public class ResetPasswordTokens
    {

        private ResetPasswordTokens(Guid id, Guid userId, string tokenHash, DateTime expiresAt)
        {
            Id = id;
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
        }

        // Фабричный метод для создания объекта
        public static ResetPasswordTokens Create(Guid id, Guid userId, string tokenHash, DateTime expiresAt)
        {
            return new ResetPasswordTokens(id, userId, tokenHash, expiresAt);
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string TokenHash { get; set; }
        public User User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
