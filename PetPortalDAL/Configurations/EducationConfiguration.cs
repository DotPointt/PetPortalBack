using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности EducationEntity в базе данных.
/// </summary>
public class EducationConfiguration : IEntityTypeConfiguration<EducationEntity>
{
    /// <summary>
    /// Настройка конфигурации для сущности EducationEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<EducationEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.University)
            .IsRequired();
        
        builder.Property(e => e.Speciality)
            .IsRequired();
        
        builder.Property(e => e.ReleaseYear)
            .IsRequired();
    }
}