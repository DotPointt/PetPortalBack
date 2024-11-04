using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalCore.Models;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations
{
    /// <summary>
    /// Project configurations in data base.
    /// </summary>
    public class ProjectConfigurations : IEntityTypeConfiguration<ProjectEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {
            builder.HasKey(project => project.Id);

            builder.Property(project => project.Name)
                .HasMaxLength(Project.MAX_NAME_LENGHT)
                .IsRequired();

            builder.Property(project => project.Description)
                .IsRequired();

            builder.Property(project => project.OwnerId)
                .IsRequired();
        }
    }
}