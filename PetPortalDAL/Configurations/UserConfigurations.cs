using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// User configurations in data base.
/// </summary>
public class UserConfigurations : IEntityTypeConfiguration<UserEntity>
{
    /// <summary>
    /// User entity configuration in data base.
    /// </summary>
    /// <param name="builder">Configurator.</param>
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Name)
            .IsRequired();

        builder.Property(user => user.Email)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .IsRequired();
        
        builder.Property(user => user.AvatarUrl)
            .IsRequired();
    }
}