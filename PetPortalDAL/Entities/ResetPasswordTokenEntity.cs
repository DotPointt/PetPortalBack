using PetPortalCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalDAL.Entities
{
    public class ResetPasswordTokenEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string TokenHash { get; set; }
        public UserEntity User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
