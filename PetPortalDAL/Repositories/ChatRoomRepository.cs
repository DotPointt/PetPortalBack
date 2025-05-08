using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
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

    public async Task<ChatRoomDto?> GetByIdAsync(Guid roomId)
    {
        var entity = await _context.ChatRooms
            .Include(r => r.ChatRoomUsers)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (entity is null) return null;

        return new ChatRoomDto
        {
            Id = entity.Id,
            Name = entity.Name,
            UserIds = entity.ChatRoomUsers.Select(u => u.UserId).ToList()
        };
    }

    public async Task<List<ChatRoomDto>> GetUserChatRoomsAsync(Guid userId)
    {
        return await _context.ChatRooms
            .Include(r => r.ChatRoomUsers)
            .Where(r => r.ChatRoomUsers.Any(u => u.UserId == userId))
            .Select(r => new ChatRoomDto
            {
                Id = r.Id,
                Name = r.Name,
                UserIds = r.ChatRoomUsers.Select(u => u.UserId).ToList()
            })
            .ToListAsync();
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
        var chat = new ChatRoomEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            ChatRoomUsers = userIds.Select(id => new ChatRoomUserEntity { UserId = id }).ToList()
        };

        _context.ChatRooms.Add(chat);
        await _context.SaveChangesAsync();

        return new ChatRoomDto
        {
            Id = chat.Id,
            Name = chat.Name,
            UserIds = userIds
        };
    }
}