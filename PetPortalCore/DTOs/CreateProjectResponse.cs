namespace PetPortalCore.DTOs;

/// <summary>
/// Ответ на создание проекта с опциональной ссылкой на оплату размещения.
/// </summary>
public class CreateProjectResponse
{
    public Guid ProjectId { get; set; }
    public string? PaymentUrl { get; set; }
    public bool RequiresPayment { get; set; }
    public int ProjectsCount { get; set; }
    public int FreeProjectsLimit { get; set; } = 5;
    public int FreeProjectsRemaining { get; set; }
}

/// <summary>
/// Квота бесплатных размещений проектов.
/// </summary>
public class PlacementQuotaDto
{
    public int ProjectsCount { get; set; }
    public int FreeProjectsLimit { get; set; } = 5;
    public int FreeProjectsRemaining { get; set; }
    public bool NextProjectRequiresPayment { get; set; }
}
