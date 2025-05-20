using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Репозиторий стэка пользователя.
/// </summary>
public class StackRepository : IStackRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly PetPortalDbContext _context;

    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public StackRepository(PetPortalDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Получить стэк пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список стэка пользователя.</returns>
    public async Task<List<StackDto>> GetByUserId(Guid userId)
    {
        var stacks = await _context.Stacks
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .ToListAsync();
        
        var dto = stacks
            .Select(stack => new StackDto()
            {
                Id = stack.Id,
                ProgrammingLanguage = stack.ProgrammingLanguage,
                ProgrammingLevel = stack.ProgrammingLevel,
                ProgrammingYears = stack.ProgrammingYears,
                UserId = stack.UserId,
            })
            .ToList();
        
        return dto;
    }
    
    /// <summary>
    /// Добавить стэк.
    /// </summary>
    /// <param name="stackDtos">Список стэков пользователя.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    public async Task CreateStacks(List<StackDto> stackDtos, Guid userId)
    {
        var stackEntities = stackDtos
            .Select(stack => new StackEntity()
            {
                Id = stack.Id,
                ProgrammingLanguage = stack.ProgrammingLanguage,
                ProgrammingLevel = stack.ProgrammingLevel,
                ProgrammingYears = stack.ProgrammingYears,
                UserId = userId,
            })
            .ToList();
        
        await _context.Stacks.AddRangeAsync(stackEntities);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Обновление стэка пользователя.
    /// </summary>
    /// <param name="stackDto">Стэк.</param>
    /// <returns>Идентификатор обновленного стэка.</returns>
    public async Task<Guid> UpdateStack(StackDto stackDto)
    {
        var existingStackEntity = await _context.Stacks
            .FirstOrDefaultAsync(s => s.Id == stackDto.Id);

        var stackEntity = new StackEntity()
        {
            Id = stackDto.Id,
            ProgrammingLanguage = stackDto.ProgrammingLanguage,
            ProgrammingLevel = stackDto.ProgrammingLevel,
            ProgrammingYears = stackDto.ProgrammingYears,
            UserId = stackDto.UserId,
        };
        
        _context.Entry(existingStackEntity).CurrentValues.SetValues(stackEntity);
        
        await _context.SaveChangesAsync();

        return stackDto.Id;
    }
    
    /// <summary>
    /// Удаление стэка пользователя.
    /// </summary>
    /// <param name="stackId">Идентификатор стэка пользователя.</param>
    public async Task DeleteExperience(Guid stackId)
    {
        await _context.Stacks
            .Where(s => s.Id == stackId)
            .ExecuteDeleteAsync();
    }
}