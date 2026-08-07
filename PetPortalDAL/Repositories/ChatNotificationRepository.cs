using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

public class ChatNotificationRepository : IChatNotificationRepository
{
    private readonly PetPortalDbContext _context;

    public ChatNotificationRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    public async Task<List<UnreadChatEmailCandidateDto>> GetUnreadMessagesForEmailAsync(TimeSpan minUnreadAge)
    {
        var cutoff = DateTime.UtcNow - minUnreadAge;

        var query =
            from message in _context.ChatMessages.AsNoTracking()
            join membership in _context.ChatRoomUsers.AsNoTracking()
                on message.ChatRoomId equals membership.ChatRoomId
            join recipient in _context.Users.AsNoTracking()
                on membership.UserId equals recipient.Id
            join sender in _context.Users.AsNoTracking()
                on message.SenderId equals sender.Id
            where message.SenderId != membership.UserId
                  && message.SentAt <= cutoff
                  && (membership.LastReadAt == null || membership.LastReadAt < message.SentAt)
                  && !_context.ChatMessageEmailNotifications.Any(n =>
                      n.MessageId == message.Id && n.UserId == membership.UserId)
            select new UnreadChatEmailCandidateDto
            {
                MessageId = message.Id,
                ChatRoomId = message.ChatRoomId,
                RecipientUserId = recipient.Id,
                RecipientEmail = recipient.Email,
                RecipientName = recipient.Name,
                SenderName = sender.Name,
                MessagePreview = message.Message
            };

        return await query.ToListAsync();
    }

    public async Task LogEmailSentAsync(Guid messageId, Guid userId)
    {
        var exists = await _context.ChatMessageEmailNotifications
            .AnyAsync(n => n.MessageId == messageId && n.UserId == userId);
        if (exists) return;

        await _context.ChatMessageEmailNotifications.AddAsync(new ChatMessageEmailNotificationEntity
        {
            Id = Guid.NewGuid(),
            MessageId = messageId,
            UserId = userId,
            SentAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }
}
