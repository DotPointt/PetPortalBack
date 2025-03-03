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
    public class ProjectTagConfiguration : IEntityTypeConfiguration<ProjectTag>
    {
        public void Configure(EntityTypeBuilder<ProjectTag> builder)
        {
            builder.HasKey(pf => new { pf.ProjectId, pf.TagId }); // Составной ключ

            builder.HasOne(pf => pf.Project)
                  .WithMany(p => p.ProjectTags)
                  .HasForeignKey(pf => pf.ProjectId);

            builder.HasOne(pf => pf.Tag)
                  .WithMany(f => f.ProjectTags)
                  .HasForeignKey(pf => pf.TagId);

            // Дополнительные настройки, если нужно
            builder.Property(pf => pf.ProjectId).IsRequired();
            builder.Property(pf => pf.TagId).IsRequired();
        }   
    }
}
