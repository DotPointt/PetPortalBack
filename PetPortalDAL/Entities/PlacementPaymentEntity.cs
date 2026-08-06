namespace PetPortalDAL.Entities;

/// <summary>
/// Платёж за размещение проекта.
/// </summary>
public class PlacementPaymentEntity
{
    public Guid Id { get; set; }
    public string YooKassaPaymentId { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RUB";
    public string Status { get; set; } = "pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }

    public ProjectEntity? Project { get; set; }
}
