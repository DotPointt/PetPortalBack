namespace PetPortalCore.Helpers;

/// <summary>
/// Вспомогательный класс для валидации данных.
/// </summary>
public class ValidationHelper
{
    /// <summary>
    /// Проверяет, что GUID не пустой.
    /// </summary>
    /// <param name="id">GUID для проверки.</param>
    /// <exception cref="ArgumentException">Выбрасывается, если GUID пустой.</exception>
    public void CheckGuidIsNonEmpty(Guid id)
    {
        if (Guid.Empty == id)
        {
            throw new ArgumentException("Guid is empty.", nameof(id));
        }
    }

    /// <summary>
    /// Проверяет, что строка не является null или пустой.
    /// </summary>
    /// <param name="checkedString">Строка для проверки.</param>
    /// <exception cref="ArgumentException">Выбрасывается, если строка null или пустая.</exception>
    public void CheckStringIsNonEmpty(string checkedString)
    {
        if (string.IsNullOrEmpty(checkedString))
        {
            throw new ArgumentException("String is null or empty.", nameof(checkedString));
        }
    }

    /// <summary>
    /// Проверяет, что объект не является null.
    /// </summary>
    /// <param name="checkedObject">Объект для проверки.</param>
    /// <exception cref="NullReferenceException">Выбрасывается, если объект null.</exception>
    public void CheckObjectIsNonNull(object? checkedObject)
    {
        if (checkedObject is null)
        {
            throw new NullReferenceException($"{checkedObject} is null.");
        }
    }
}