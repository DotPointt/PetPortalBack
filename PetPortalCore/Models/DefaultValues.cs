namespace PetPortalCore.Models;

/// <summary>
/// Класс для хранения значений по умолчанию.
/// </summary>
public static class DefaultValues
{
    /// <summary>
    /// Идентификатор роли по умолчанию для нового пользователя.
    /// </summary>
    public static Guid RoleId { get; private set; } = Guid.Parse("A0000000-0000-0000-0000-000000000000");
}