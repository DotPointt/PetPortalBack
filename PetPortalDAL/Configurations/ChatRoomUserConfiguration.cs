using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Configurations;

/// <summary>
/// Конфигурация для сущности ChatRoomUserEntity// в базе данных.
/// </summary>
public class ChatRoomUserConfiguration : IEntityTypeConfiguration<ChatRoomUserEntity>
{
    /// <summary>
    /// Конфигурация для сущности ChatRoomUserEntity.
    /// </summary>
    /// <param name="builder">Строитель для настройки сущности.</param>
    public void Configure(EntityTypeBuilder<ChatRoomUserEntity> builder)
    {
        builder.HasKey(cru => new { cru.ChatRoomId, cru.UserId });

        builder.HasOne(cru => cru.ChatRoom)
            .WithMany(cr => cr.ChatRoomUsers)
            .HasForeignKey(cru => cru.ChatRoomId);

        builder.HasOne(cru => cru.User)
            .WithMany()
            .HasForeignKey(cru => cru.UserId);
    }
}
