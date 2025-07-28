using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности RespondEntity в базе данных.
/// </summary>
public class RespondConfiguration : IEntityTypeConfiguration<RespondEntity>
{
    /// <summary>
    /// Настройка конфигурации для сущности RespondEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<RespondEntity> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Role)
            .IsRequired();
        
        builder.Property(r => r.UserId)
            .IsRequired();
        
        builder.Property(r => r.ProjectId)
            .IsRequired();
    }
}