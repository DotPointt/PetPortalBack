using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Configurations;

public class ProjectRoleConfiguration : IEntityTypeConfiguration<ProjectRole>
{
    /// <summary>
    /// Настройка конфигурации для сущности ProjectTag.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<ProjectRole> builder)
    {
        builder.HasKey(pf => new { pf.ProjectId, pf.RoleId });

        builder.HasOne(pf => pf.Project)
            .WithMany(p => p.ProjectRoles)
            .HasForeignKey(pf => pf.ProjectId);

        builder.HasOne(pf => pf.Role)
            .WithMany(f => f.ProjectRoles)
            .HasForeignKey(pf => pf.RoleId);

        builder.Property(pf => pf.ProjectId).IsRequired(); 
        builder.Property(pf => pf.RoleId).IsRequired(); 
    }
}