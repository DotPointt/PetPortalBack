using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Репозитория для работы с сообщениями в чатах.
/// </summary>
public class ChatMessageRepository : IChatMessageRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly PetPortalDbContext _context;

    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public ChatMessageRepository(PetPortalDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Записать сообщение в базу данных.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <returns>Идентификатор сообщения.</returns>
    public async Task<Guid> AddAsync(ChatMessageDto message)
    {
        await _context.ChatMessages.AddAsync(new ChatMessageEntity()
        {
            Id = message.Id,
            ChatRoom = message.ChatRoom,
            Message = message.Message,
            Username = message.Username,
            SentAt = message.SentAt
        });
        
        await _context.SaveChangesAsync();
        return message.Id;
    }

    /// <summary>
    /// Удалить сообщение из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор сообщения.</param>
    /// <returns>Идентификатор удаленного сообщения.</returns>
    public async Task<Guid> DeleteAsync(Guid id)
    {
        await _context.ChatMessages
            .Where(message => message.Id == id)
            .FirstOrDefaultAsync();
        
        return id;
    }

    /// <summary>
    /// Получить сообщение по идентификатору. 
    /// </summary>
    /// <param name="id">Идентификатор сообщения.</param>
    /// <returns>Сообщение (или null пока).</returns>
    public async Task<ChatMessageDto> GetByIdAsync(Guid id)
    {
        var message = await _context.ChatMessages.FindAsync(id);
        
        if (message == null)
            throw new NullReferenceException();

        return new ChatMessageDto()
        {
            Id = message.Id,
            Message = message.Message,
            Username = message.Username,
            ChatRoom = message.ChatRoom,
            SentAt = message.SentAt
        };
    }

    /// <summary>
    /// Получить сообщения по названию комнаты.
    /// </summary>
    /// <param name="chatRoom">Название чата.</param>
    /// <returns>Список сообщений чата.</returns>
    public async Task<List<ChatMessageDto>> GetMessagesByRoomAsync(string chatRoom)
    {
        var messageEntities = await _context.ChatMessages
            .AsNoTracking()
            .Where(m => m.ChatRoom == chatRoom)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
        
        var messages = messageEntities
            .Select(m => 
                new ChatMessageDto
                {
                    Id = m.Id,
                    Message = m.Message, 
                    Username = m.Username,
                    ChatRoom = m.ChatRoom, 
                    SentAt = m.SentAt
                })
            .ToList();
        
        return messages;
    }

    /// <summary>
    /// Получить <paramref name="count"/> сообщений чата <paramref name="chatRoom"/>.
    /// </summary>
    /// <param name="chatRoom">Название чата.</param>
    /// <param name="count">Количество сообщений.</param>
    /// <returns>Список сообщений по параметрам.</returns>
    public async Task<List<ChatMessageDto>> GetLastMessagesAsync(string chatRoom, int count)
    {
        var messageEntities = await _context.ChatMessages
            .AsNoTracking()
            .Where(m => m.ChatRoom == chatRoom)
            .OrderByDescending(m => m.SentAt)
            .Take(count)
            .ToListAsync();
        
        var messages = messageEntities
            .Select(m => 
                new ChatMessageDto
                {
                    Id = m.Id,
                    Message = m.Message, 
                    Username = m.Username,
                    ChatRoom = m.ChatRoom, 
                    SentAt = m.SentAt
                }
            ).ToList();
        
        return messages;
    }
}