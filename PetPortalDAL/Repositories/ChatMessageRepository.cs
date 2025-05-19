using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalDAL;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly PetPortalDbContext _context;

    public ChatMessageRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(ChatMessageDto message)
    {
        var messageEntity = new ChatMessageEntity()
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ChatRoomId = message.ChatRoomId,
            Message = message.Message,
            SentAt = message.SentAt,
        };
            
        await _context.ChatMessages.AddAsync(messageEntity);
        await _context.SaveChangesAsync();
        return message.Id;
    }

    public async Task<List<ChatMessageDto>> GetMessagesByRoomIdAsync(Guid roomId)
    {
        var messageEntities = await _context.ChatMessages
            .Where(m => m.ChatRoomId == roomId)
            .Include(m => m.Sender)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        var messages = messageEntities
            .Select(message => new ChatMessageDto()
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ChatRoomId = message.ChatRoomId,
                Message = message.Message,
                SentAt = message.SentAt,
            })
            .ToList();
        
        return messages;
    }

    public async Task<List<ChatMessageDto>> GetLastMessagesAsync(Guid roomId, int count)
    {
        var messageEntities= await _context.ChatMessages
            .Where(m => m.ChatRoomId == roomId)
            .Include(m => m.Sender)
            .OrderByDescending(m => m.SentAt)
            .Take(count)
            .ToListAsync();
        
        var messages = messageEntities
            .Select(message => new ChatMessageDto()
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ChatRoomId = message.ChatRoomId,
                Message = message.Message,
                SentAt = message.SentAt,
            })
            .ToList();
        
        return messages;
    }

    public async Task<ChatMessageDto?> GetByIdAsync(Guid messageId)
    {
       var messageEntity = await _context.ChatMessages
            .Include(m => m.Sender)
            .FirstOrDefaultAsync(m => m.Id == messageId);

       return messageEntity == null
           ? null
           : new ChatMessageDto()
           {
               Id = messageEntity.Id,
               SenderId = messageEntity.SenderId,
               ChatRoomId = messageEntity.ChatRoomId,
               Message = messageEntity.Message,
               SentAt = messageEntity.SentAt,
           };
    }

    public async Task<Guid> DeleteAsync(Guid messageId)
    {
        var entity = await _context.ChatMessages.FindAsync(messageId);
        if (entity != null)
        {
            _context.ChatMessages.Remove(entity);
            await _context.SaveChangesAsync();
        }

        return messageId;
    }
}