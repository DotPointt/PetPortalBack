namespace PetPortalCore.DTOs;

public class UnreadChatEmailCandidateDto
{
    public Guid MessageId { get; set; }
    public Guid ChatRoomId { get; set; }
    public Guid RecipientUserId { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string MessagePreview { get; set; } = string.Empty;
}
