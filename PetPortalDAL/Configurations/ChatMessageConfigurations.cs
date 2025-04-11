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
        builder.HasKey(message => message.Id);
        
        builder.Property(message => message.Username)
            .IsRequired();
        
        builder.Property(message => message.ChatRoom)
            .IsRequired();
        
        builder.Property(message => message.Message)
            .IsRequired();
        
        builder.Property(message => message.SentAt)
            .IsRequired();
    }
}