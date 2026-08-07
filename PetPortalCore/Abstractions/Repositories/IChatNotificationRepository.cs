using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Repositories;

public interface IChatNotificationRepository
{
    Task<List<UnreadChatEmailCandidateDto>> GetUnreadMessagesForEmailAsync(TimeSpan minUnreadAge);
    Task LogEmailSentAsync(Guid messageId, Guid userId);
}
