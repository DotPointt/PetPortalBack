using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности ProjectTag (связующая таблица между проектами и тегами).
/// </summary>
public class ProjectTagConfiguration : IEntityTypeConfiguration<ProjectTag>
{
    /// <summary>
    /// Настройка конфигурации для сущности ProjectTag.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<ProjectTag> builder)
    {
        builder.HasKey(pf => new { pf.ProjectId, pf.TagId });

        builder.HasOne(pf => pf.Project)
            .WithMany(p => p.ProjectTags)
            .HasForeignKey(pf => pf.ProjectId);

        builder.HasOne(pf => pf.Tag)
            .WithMany(f => f.ProjectTags)
            .HasForeignKey(pf => pf.TagId);

        builder.Property(pf => pf.ProjectId).IsRequired(); 
        builder.Property(pf => pf.TagId).IsRequired(); 
    }
}