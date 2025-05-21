using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Репозиторий образований пользователя.
/// </summary>
public class EducationRepository : IEducationRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly PetPortalDbContext _context;

    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public EducationRepository(PetPortalDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Получить образования пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список образований.</returns>
    public async Task<List<EducationDto>> GetByUserId(Guid userId)
    {
        var educations = await _context.Educations
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .ToListAsync();
        
        var dto = educations
            .Select(education => new EducationDto()
            {
                Id = education.Id,
                ReleaseYear = education.ReleaseYear,
                Speciality = education.Speciality,
                University = education.University,
                UserId = education.UserId,
            })
            .ToList();
        
        return dto;
    }

    /// <summary>
    /// Создать новые образования.
    /// </summary>
    /// <param name="education">Образования.</param>
    public async Task CreateEducation(EducationDto education)
    {
        var educationEntity = new EducationEntity()
        {
            Id = education.Id,
            ReleaseYear = education.ReleaseYear,
            Speciality = education.Speciality,
            University = education.University,
            UserId = education.UserId,
        };
            
        await _context.Educations.AddAsync(educationEntity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновление образования пользователя.
    /// </summary>
    /// <param name="education">Образование.</param>
    /// <returns>Идентификатор обновленного образования.</returns>
    public async Task<Guid> UpdateEducation(EducationDto education)
    {
        var existingEducationEntity = await _context.Educations
            .FirstOrDefaultAsync(e => e.Id == education.Id);

        if (existingEducationEntity == null)
        {
            throw new KeyNotFoundException("Education not found");
        }
        
        var educationEntity = new EducationEntity()
        {
            Id = education.Id,
            ReleaseYear = education.ReleaseYear,
            Speciality = education.Speciality,
            University = education.University,
            UserId = education.UserId,
        };
        
        _context.Entry(existingEducationEntity).CurrentValues.SetValues(educationEntity);
        
        await _context.SaveChangesAsync();

        return education.Id;
    }
    
    /// <summary>
    /// Удаление образования пользователя.
    /// </summary>
    /// <param name="educationId">Идентификатор образования.</param>
    public async Task DeleteEducation(Guid educationId)
    {
        await _context.Educations
            .Where(e => e.Id == educationId)
            .ExecuteDeleteAsync();
    }
}