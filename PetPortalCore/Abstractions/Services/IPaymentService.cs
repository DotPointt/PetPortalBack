namespace PetPortalCore.Abstractions.Services;

public interface IPaymentService
{
    /// <summary>
    /// Создать платёж за размещение проекта. Возвращает ConfirmationUrl.
    /// </summary>
    Task<string> CreatePlacementPaymentAsync(Guid projectId, Guid userId);

    /// <summary>
    /// Опросить YooKassa и при успехе открыть проект.
    /// </summary>
    Task HandlePaymentAsync(string yooKassaPaymentId);

    /// <summary>
    /// Обработать уведомление/подтверждение оплаты и открыть проект.
    /// </summary>
    Task<bool> ConfirmPaymentAndPublishAsync(string yooKassaPaymentId);
}
