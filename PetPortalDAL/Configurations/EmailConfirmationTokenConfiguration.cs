using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

public class EmailConfirmationTokenConfiguration : IEntityTypeConfiguration<EmailConfirmationTokenEntity>
{
    public void Configure(EntityTypeBuilder<EmailConfirmationTokenEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasIndex(t => t.UserId).IsUnique();
        builder.Property(t => t.TokenHash).IsRequired();
        builder.Property(t => t.ExpiresAt).IsRequired();
    }
}
