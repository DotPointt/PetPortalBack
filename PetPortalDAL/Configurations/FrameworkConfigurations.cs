using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalDAL.Configurations
{
    public class FrameworkConfigurations : IEntityTypeConfiguration<FrameworkEntity>
    {
        public void Configure(EntityTypeBuilder<FrameworkEntity> builder)
        {
            builder.HasKey(Framework => Framework.Id);

            builder.Property(Framework => Framework.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(f => f.ProjectFrameworks)
              .WithOne(pf => pf.Framework)
              .HasForeignKey(pf => pf.FrameworkId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
