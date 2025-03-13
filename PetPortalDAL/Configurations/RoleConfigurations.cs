using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности RoleEntity в базе данных.
/// </summary>
public class RoleConfigurations : IEntityTypeConfiguration<RoleEntity>
{
    /// <summary>
    /// Настройка конфигурации для сущности RoleEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(role => role.Id);

        builder.Property(role => role.Name)
            .IsRequired(); 
    }
}