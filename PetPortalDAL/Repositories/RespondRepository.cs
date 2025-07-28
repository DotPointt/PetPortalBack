using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Репозиторий для работы с откликами.
/// </summary>
public class RespondRepository : IRespondRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly PetPortalDbContext _context;
        
    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public RespondRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<List<RespondDto>> GetAllResponds()
    {
        var responds = await _context.Responds
            .AsNoTracking()
            .ToListAsync();

        var respondDtos = responds
            .Select(r => new RespondDto
            {
                Id = r.Id,
                Role = r.Role,
                Comment = r.Comment,
                UserId = r.UserId,
                ProjectId = r.ProjectId,
            })
            .ToList();
        
        return respondDtos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<RespondDto>> GetRespondsByUserId(Guid userId)
    {
        var responds = await _context.Responds
            .AsNoTracking()
            .Where(respond => respond.UserId == userId)
            .ToListAsync();
        
        var respondDtos = responds
            .Select(r => new RespondDto
            {
                Id = r.Id,
                Role = r.Role,
                Comment = r.Comment,
                UserId = r.UserId,
                ProjectId = r.ProjectId,
            })
            .ToList();
        
        return respondDtos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    public async Task<List<RespondDto>> GetRespondsByProjectId(Guid projectId)
    {
        var responds = await _context.Responds
            .AsNoTracking()
            .Where(respond => respond.ProjectId == projectId)
            .ToListAsync();
        
        var respondDtos = responds
            .Select(r => new RespondDto
            {
                Id = r.Id,
                Role = r.Role,
                Comment = r.Comment,
                UserId = r.UserId,
                ProjectId = r.ProjectId,
            })
            .ToList();
        
        return respondDtos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="respondDto"></param>
    /// <returns></returns>
    public async Task<bool> CreateRespond(RespondDto respondDto)
    {
        var respondEntity = new RespondEntity()
        {
            Id = respondDto.Id,
            UserId = respondDto.UserId,
            ProjectId = respondDto.ProjectId,
            Comment = respondDto.Comment,
            Role = respondDto.Role,
        };

        try
        {
            await _context.AddAsync(respondEntity);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="respondId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> DeleteRespond(Guid respondId)
    {
        try
        {
            await _context.Responds
                .Where(respond => respond.Id == respondId)
                .ExecuteDeleteAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
        
    }
}