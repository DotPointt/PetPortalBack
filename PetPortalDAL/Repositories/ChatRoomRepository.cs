using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalDAL;
using PetPortalDAL.Entities;
using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Repositories;

public class ChatRoomRepository : IChatRoomRepository
{
    private readonly PetPortalDbContext _context;

    public ChatRoomRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    private async Task<ChatRoomDto> MapRoomAsync(ChatRoomEntity entity)
    {
        var userIds = entity.ChatRoomUsers.Select(u => u.UserId).ToList();
        var names = entity.ChatRoomUsers
            .Where(u => u.User != null)
            .ToDictionary(u => u.UserId, u => u.User.Name ?? "Пользователь");

        // Fallback load names if navigation wasn't included
        var missing = userIds.Where(id => !names.ContainsKey(id)).ToList();
        if (missing.Count > 0)
        {
            var users = await _context.Users
                .AsNoTracking()
                .Where(u => missing.Contains(u.Id))
                .Select(u => new { u.Id, u.Name })
                .ToListAsync();
            foreach (var u in users)
                names[u.Id] = u.Name ?? "Пользователь";
        }

        var last = await _context.ChatMessages
            .AsNoTracking()
            .Where(m => m.ChatRoomId == entity.Id)
            .OrderByDescending(m => m.SentAt)
            .Select(m => new { m.Message, m.SentAt })
            .FirstOrDefaultAsync();

        return new ChatRoomDto
        {
            Id = entity.Id,
            Name = entity.Name,
            UserIds = userIds,
            ParticipantNames = names,
            LastMessage = last?.Message,
            LastMessageTime = last?.SentAt
        };
    }

    public async Task<ChatRoomDto?> GetByIdAsync(Guid roomId)
    {
        var entity = await _context.ChatRooms
            .Include(r => r.ChatRoomUsers)
                .ThenInclude(u => u.User)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (entity is null) return null;
        return await MapRoomAsync(entity);
    }

    public async Task<List<ChatRoomDto>> GetUserChatRoomsAsync(Guid userId)
    {
        var entities = await _context.ChatRooms
            .Include(r => r.ChatRoomUsers)
                .ThenInclude(u => u.User)
            .Where(r => r.ChatRoomUsers.Any(u => u.UserId == userId))
            .ToListAsync();

        var result = new List<ChatRoomDto>();
        foreach (var entity in entities)
            result.Add(await MapRoomAsync(entity));
        return result.OrderByDescending(r => r.LastMessageTime ?? DateTime.MinValue).ToList();
    }

    public async Task<Guid?> GetChatRoomIdByNameAsync(string name)
    {
        return await _context.ChatRooms
            .Where(r => r.Name == name)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<ChatRoomDto> CreateNamedChatAsync(string name, List<Guid> userIds)
    {
        if (!userIds.Any())
            throw new ArgumentException("Пользователи обязательны", nameof(userIds));

        var existingChat = await _context.ChatRooms
            .Where(r => r.Name == name)
            .Select(r => new { r.Id })
            .FirstOrDefaultAsync();

        if (existingChat != null)
            throw new InvalidOperationException($"Такой чат уже существует.");

        var chat = new ChatRoomEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            ChatRoomUsers = userIds.Distinct().Select(id => new ChatRoomUserEntity { UserId = id }).ToList()
        };

        _context.ChatRooms.Add(chat);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(chat.Id) ?? new ChatRoomDto
        {
            Id = chat.Id,
            Name = chat.Name,
            UserIds = userIds.Distinct().ToList()
        };
    }

    public async Task MarkRoomAsReadAsync(Guid roomId, Guid userId)
    {
        var membership = await _context.ChatRoomUsers
            .FirstOrDefaultAsync(m => m.ChatRoomId == roomId && m.UserId == userId);
        if (membership == null) return;

        membership.LastReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
