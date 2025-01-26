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

        builder.HasData(
            new UserEntity
            {
                Id = Guid.NewGuid(), // Генерация уникального идентификатора
                Name = "John Doe",
                Email = "johndoe@example.com",
                PasswordHash = "hashedpassword1" // Здесь предполагается хэш пароля
            },
            new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Jane Smith",
                Email = "janesmith@example.com",
                PasswordHash = "hashedpassword2"
            },
            new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Alex Johnson",
                Email = "alexjohnson@example.com",
                PasswordHash = "hashedpassword3"
            }
        );
    }
}