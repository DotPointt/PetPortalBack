using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности TagEntity в базе данных.
/// </summary>
public class TagConfigurations : IEntityTypeConfiguration<TagEntity>
{
    /// <summary>
    /// Настройка конфигурации для сущности TagEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder.HasKey(tag => tag.Id);

        builder.Property(tag => tag.Name)
            .IsRequired() 
            .HasMaxLength(255);
        
        
        builder.HasMany(f => f.ProjectTags)
            .WithOne(pf => pf.Tag)
            .HasForeignKey(pf => pf.TagId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}