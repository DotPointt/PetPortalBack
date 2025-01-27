using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Project configurations in data base.
/// </summary>
public class RoleConfigurations : IEntityTypeConfiguration<RoleEntity>
{
    /// <summary>
    /// Role entity configuration in data base.
    /// </summary>
    /// <param name="builder">Configurator.</param>
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(role => role.Id);

        builder.Property(role => role.Name)
            .IsRequired();
    }
}