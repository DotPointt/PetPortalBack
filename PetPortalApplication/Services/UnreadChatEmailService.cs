using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalApplication.Helpers;

namespace PetPortalApplication.Services;

public interface IUnreadChatEmailService
{
    Task ProcessPendingAsync(CancellationToken cancellationToken = default);
}

public class UnreadChatEmailService : IUnreadChatEmailService
{
    private static readonly TimeSpan MinUnreadAge = TimeSpan.FromHours(2);

    private readonly IChatNotificationRepository _notificationRepository;
    private readonly IMailSenderService _mailSender;

    public UnreadChatEmailService(
        IChatNotificationRepository notificationRepository,
        IMailSenderService mailSender)
    {
        _notificationRepository = notificationRepository;
        _mailSender = mailSender;
    }

    public async Task ProcessPendingAsync(CancellationToken cancellationToken = default)
    {
        var candidates = await _notificationRepository.GetUnreadMessagesForEmailAsync(MinUnreadAge);
        var chatLink = $"{AppUrls.FrontendBase}/chat";

        foreach (var item in candidates)
        {
            if (cancellationToken.IsCancellationRequested) break;
            if (string.IsNullOrWhiteSpace(item.RecipientEmail)) continue;

            try
            {
                var preview = item.MessagePreview.Length > 200
                    ? item.MessagePreview[..200] + "…"
                    : item.MessagePreview;

                var body = EmailTemplates.UnreadChatMessage(
                    item.RecipientName,
                    item.SenderName,
                    preview,
                    chatLink);

                await _mailSender.SendEmailAsync(
                    item.RecipientEmail,
                    "Непрочитанное сообщение в чате — PetPortal",
                    body,
                    isBodyHtml: true);

                await _notificationRepository.LogEmailSentAsync(item.MessageId, item.RecipientUserId);
            }
            catch
            {
                // Пропускаем отдельное письмо, не блокируем остальные
            }
        }
    }
}
