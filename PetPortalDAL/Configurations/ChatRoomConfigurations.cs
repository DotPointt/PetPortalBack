using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности ChatRoom в базе данных.
/// </summary>
public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoomEntity>
{
    /// <summary>
    /// Конфигурация для сущности ChatRoomEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<ChatRoomEntity> builder)
    {
        builder.HasKey(cr => cr.Id);
        builder.Property(cr => cr.Name).HasMaxLength(100);

        builder.HasMany(cr => cr.ChatRoomUsers)
            .WithOne(cru => cru.ChatRoom)
            .HasForeignKey(cru => cru.ChatRoomId);

        builder.HasMany(cr => cr.Messages)
            .WithOne(m => m.ChatRoom)
            .HasForeignKey(m => m.ChatRoomId);
    }
}
