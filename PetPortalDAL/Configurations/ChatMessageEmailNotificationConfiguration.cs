using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

public class ChatMessageEmailNotificationConfiguration : IEntityTypeConfiguration<ChatMessageEmailNotificationEntity>
{
    public void Configure(EntityTypeBuilder<ChatMessageEmailNotificationEntity> builder)
    {
        builder.HasKey(n => n.Id);
        builder.HasIndex(n => new { n.MessageId, n.UserId }).IsUnique();

        builder.HasOne(n => n.Message)
            .WithMany()
            .HasForeignKey(n => n.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
