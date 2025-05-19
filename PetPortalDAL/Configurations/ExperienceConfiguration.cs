using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности ExperienceEntity в базе данных.
/// </summary>
public class ExperienceConfiguration : IEntityTypeConfiguration<ExperienceEntity>
{
    /// <summary>
    /// Настройка конфигурации для сущности ExperienceEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<ExperienceEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.WorkPlace)
            .IsRequired();
        
        builder.Property(x => x.WorkPosition)
            .IsRequired();
        
        builder.Property(x => x.WorkYears)
            .IsRequired();
    }
}