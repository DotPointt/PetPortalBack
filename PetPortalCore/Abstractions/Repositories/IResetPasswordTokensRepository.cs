using PetPortalCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalCore.Abstractions.Repositories
{
    public interface IResetPasswordTokensRepository
    {
        public Task<Guid> SaveTokenHash(ResetPasswordTokens token);

        public Task<ResetPasswordTokens> GetTokenHashByUserId(Guid userId);
    }
}
