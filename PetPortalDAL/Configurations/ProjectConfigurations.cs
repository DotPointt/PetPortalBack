using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Project configurations in data base.
/// </summary>
public class ProjectConfigurations : IEntityTypeConfiguration<ProjectEntity>
{
    /// <summary>
    /// Project entity configuration in data base.
    /// </summary>
    /// <param name="builder">Configurator.</param>
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.HasKey(project => project.Id);

        builder.Property(project => project.Name)
            .IsRequired();

        builder.Property(project => project.Description)
            .IsRequired();

        builder.Property(project => project.OwnerId)
            .IsRequired();
    }
}