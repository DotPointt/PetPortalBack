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
    public class TagConfigurations : IEntityTypeConfiguration<TagEntity>
    {
        public void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder.HasKey(Tag => Tag.Id);

            builder.Property(Tag => Tag.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(f => f.ProjectTags)
              .WithOne(pf => pf.Tag)
              .HasForeignKey(pf => pf.TagId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
