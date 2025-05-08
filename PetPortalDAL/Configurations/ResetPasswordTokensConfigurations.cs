using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

public class ResetPasswordTokensConfigurations : IEntityTypeConfiguration<ResetPasswordTokenEntity>
{
    public void Configure(EntityTypeBuilder<ResetPasswordTokenEntity> builder)
    {
        builder.HasKey(token => token.Id);
        
        builder.Property(token => token.TokenHash)
            .IsRequired();
        
        builder.Property(token => token.UserId)
            .IsRequired();
        
        builder.Property(token => token.ExpiresAt)
            .IsRequired();
        
    }
}