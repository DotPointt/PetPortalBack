using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности ChatMessage в базе данных.
/// </summary>
public class ChatMessageConfigurations : IEntityTypeConfiguration<ChatMessageEntity>
{
    /// <summary>
    /// Конфигурация для сущности ChatMessageEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
    {
        builder.HasKey(cm => cm.Id);

        builder.Property(cm => cm.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(cm => cm.SentAt)
            .IsRequired();

        builder.HasOne(cm => cm.ChatRoom)
            .WithMany(cr => cr.Messages)
            .HasForeignKey(cm => cm.ChatRoomId);

        builder.HasOne(cm => cm.Sender)
            .WithMany()
            .HasForeignKey(cm => cm.SenderId);
    }
}