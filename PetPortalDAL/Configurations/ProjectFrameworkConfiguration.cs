using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;
using PetPortalDAL.Entities.LinkingTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetPortalDAL.Configurations
{
    public class ProjectFrameworkConfiguration : IEntityTypeConfiguration<ProjectFramework>
    {
        public void Configure(EntityTypeBuilder<ProjectFramework> builder)
        {
            builder.HasKey(pf => new { pf.ProjectId, pf.FrameworkId }); // Составной ключ

            builder.HasOne(pf => pf.Project)
                  .WithMany(p => p.ProjectFrameworks)
                  .HasForeignKey(pf => pf.ProjectId);

            builder.HasOne(pf => pf.Framework)
                  .WithMany(f => f.ProjectFrameworks)
                  .HasForeignKey(pf => pf.FrameworkId);

            // Дополнительные настройки, если нужно
            builder.Property(pf => pf.ProjectId).IsRequired();
            builder.Property(pf => pf.FrameworkId).IsRequired();
        }   
    }
}
