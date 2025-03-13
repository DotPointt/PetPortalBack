using System;

namespace PetPortalCore.Models;

/// <summary>
/// Класс, представляющий тег.
/// </summary>
public class Tag
{
    /// <summary>
    /// Идентификатор тега.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название тега.
    /// </summary>
    public string Name { get; set; }
}