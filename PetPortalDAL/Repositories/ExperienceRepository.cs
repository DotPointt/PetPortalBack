using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Репозиторий опыта работы пользователя.
/// </summary>
public class ExperienceRepository : IExperienceRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly PetPortalDbContext _context;

    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public ExperienceRepository(PetPortalDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Получить опыт работы пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список опыта работы.</returns>
    public async Task<List<ExperienceDto>> GetByUserId(Guid userId)
    {
        var experiences = await _context.Experiences
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .ToListAsync();
        
        var dto = experiences
            .Select(experience => new ExperienceDto()
            {
                Id = experience.Id,
                WorkPlace = experience.WorkPlace,
                WorkPosition = experience.WorkPosition,
                WorkYears = experience.WorkYears,
                UserId = experience.UserId,
            })
            .ToList();
        
        return dto;
    }

    /// <summary>
    /// Добавить опыт работы.
    /// </summary>
    /// <param name="experienceDto">Список опыта работы.</param>
    public async Task CreateExperience(ExperienceDto experienceDto)
    {
        var experienceEntity = new ExperienceEntity()
        {
            Id = experienceDto.Id,
            WorkPlace = experienceDto.WorkPlace,
            WorkPosition = experienceDto.WorkPosition,
            WorkYears = experienceDto.WorkYears,
            UserId = experienceDto.UserId,
        };
        
        await _context.Experiences.AddAsync(experienceEntity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновление опыта работы пользователя.
    /// </summary>
    /// <param name="experience">Опыт работы.</param>
    /// <returns>Идентификатор обновленного опыта работы.</returns>
    public async Task<Guid> UpdateExperience(ExperienceDto experience)
    {
        var existingExperienceEntity = await _context.Experiences
            .FirstOrDefaultAsync(e => e.Id == experience.Id);

        if (existingExperienceEntity == null)
        {
            throw new KeyNotFoundException("Experience not found");
        }
        
        var experienceEntity = new ExperienceEntity()
        {
            Id = experience.Id,
            WorkPlace = experience.WorkPlace,
            WorkPosition = experience.WorkPosition,
            WorkYears = experience.WorkYears,
            UserId = experience.UserId,
        };
        
        _context.Entry(existingExperienceEntity).CurrentValues.SetValues(experienceEntity);
        
        await _context.SaveChangesAsync();

        return experience.Id;
    }
    
    /// <summary>
    /// Удаление опыта работы пользователя.
    /// </summary>
    /// <param name="experienceId">Идентификатор опыта работы.</param>
    public async Task DeleteExperience(Guid experienceId)
    {
        await _context.Experiences
            .Where(e => e.Id == experienceId)
            .ExecuteDeleteAsync();
    }
}