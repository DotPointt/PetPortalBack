using System.ComponentModel;

namespace PetPortalCore.DTOs.Requests;

/// <summary>
/// Запрос на получение проектов.
/// </summary>
public class ProjectRequest
{
    /// <summary>
    /// Порядок проектов.
    /// True - по порядку
    /// False - в обратном порядке
    /// </summary>
    public bool SortOrder { get; set; } = false;
    
    /// <summary>
    /// Элемент, по которому сортируются проекты: date, budget, deadline…
    /// По умолчанию — дата создания (новизна).
    /// </summary>
    /// <example>date</example>
    public string? SortItem { get; set; } = "date";

    /// <summary>
    /// Поиск по строчке.
    /// </summary>
    public string SearchElement { get; set; } = string.Empty;

    /// <summary>
    /// Ограничение в количество проектов на странице.
    /// </summary>
    [DefaultValue(10)]
    public int Offset { get; set; } = 10;
    
    /// <summary>
    /// Номер страницы.
    /// </summary>
    [DefaultValue(1)]
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Фильтры
    /// </summary>
    public ProjectFilterDTO? Filters { get; set; }
}