using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности ProjectEntity в базе данных.
/// </summary>
public class ProjectConfigurations : IEntityTypeConfiguration<ProjectEntity>
{
    /// <summary>
    /// Настройка конфигурации для сущности ProjectEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.HasKey(project => project.Id);

        builder.Property(project => project.Name)
            .IsRequired() 
            .HasMaxLength(255); 

        builder.Property(project => project.Description)
            .IsRequired(); 

        builder.Property(project => project.OwnerId)
            .IsRequired(); 

        builder.HasMany(p => p.ProjectTags)
            .WithOne(pf => pf.Project)
            .HasForeignKey(pf => pf.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}