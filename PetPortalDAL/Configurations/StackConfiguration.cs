using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности StackEntity в базе данных.
/// </summary>
public class StackConfiguration : IEntityTypeConfiguration<StackEntity>
{
    /// <summary>
    /// Настройка конфигурации для сущности StackEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<StackEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ProgrammingLanguage)
            .IsRequired();
        
        builder.Property(x => x.ProgrammingLevel)
            .IsRequired()
            .HasAnnotation("Range", new[] { 1, 5 });
        
        builder.Property(x => x.ProgrammingYears)
            .IsRequired();
    }
}